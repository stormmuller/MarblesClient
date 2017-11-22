using Marbles.Enums;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;
using Marbles.Components.Levels;
using System.Linq;
using System.Collections.Generic;
using Marbles.Systems.Contracts.MarbleMechanics;

namespace Marbles.Systems
{
    public class HumanMarbleMechanicsController : IHumanMarbleMechanicsController
    {
        public MarbleShotStatus MarbleShotStatus { get; private set; }
        private float IdleVelocityThresholdSqr { get { return IdleVelocityThreshold * IdleVelocityThreshold; } }

        private const string PlaneRayCastLayerName = "Battle Ground Plane";
        private const float ScrollSpeed = 0.75f;
        private const float ShotPower = 15f;
        private const float IdleVelocityThreshold = 0.25f;
        private const float CheckForIdleDelay = 0.5f;
        private const float StopVelocityMultiplier = 0.3f;

        private readonly IInputManager inputManager;
        private readonly IPlayerSystem playerSystem;
        private readonly IOpponentSystem opponentSystem;
        private readonly ICameraController cameraController;
        private readonly ILevelLoader levelLoader;
        private readonly IBattleManager battleManager;

        private LineRenderer lineRenderer;
        private LayerMask layerMask;
        private Vector3 lastAimRaycastPoint;
        private float lastEndShot;

        public HumanMarbleMechanicsController(IInputManager inputManager
            , IPlayerSystem playerSystem
            , IOpponentSystem opponentSystem
            , ICameraController cameraController
            , ILevelLoader levelLoader
            , IBattleManager battleManager)
        {
            this.inputManager = inputManager;
            this.playerSystem = playerSystem;
            this.opponentSystem = opponentSystem;
            this.cameraController = cameraController;
            this.levelLoader = levelLoader;
            this.battleManager = battleManager;

            MarbleShotStatus = MarbleShotStatus.Idle;
            layerMask = 1 << LayerMask.NameToLayer(PlaneRayCastLayerName);

            lastEndShot = 0f;
        }

        public void Tick()
        {
            var player = playerSystem.GetPlayer();

            if (levelLoader.GetCurrentLevel().GetType() == typeof(BattleGroundLevel) && player != null)
            {
                HandleAimingOfShot();
                HandleShotStatusChangeDueToVelocity(player);
            }
        }

        private void HandleShotStatusChangeDueToVelocity(GameObject player)
        {
            var playerRigidBody = player.GetComponent<Rigidbody>();

            if (MarbleShotStatus == MarbleShotStatus.Shooting &&
                playerRigidBody.velocity.sqrMagnitude < IdleVelocityThresholdSqr &&
                Time.time - lastEndShot > CheckForIdleDelay)
            {
                var opponent = opponentSystem.GetOpponent().GetComponent<Rigidbody>();

                if (opponent.velocity.sqrMagnitude > IdleVelocityThresholdSqr)
                {
                    return;
                }

                var all = new List<Rigidbody>();
                all.Add(opponent);
                all.Add(playerRigidBody);

                StopAll(all);
                MarbleShotStatus = MarbleShotStatus.Idle;
            }
        }

        private void StopAll(List<Rigidbody> all)
        {
            all.ForEach(rb =>
            {
                rb.velocity *= StopVelocityMultiplier;
                rb.angularVelocity *= StopVelocityMultiplier;
            });

        }

        public void PrepareMarbleForShot(Component component)
        {
            if (MarbleShotStatus == MarbleShotStatus.Idle)
            {
                MarbleShotStatus = MarbleShotStatus.PreparingShot;
                cameraController.Zoom(-2f);

                lineRenderer = component.GetComponent<LineRenderer>();
                lineRenderer.enabled = true;

                HandleAimingOfShot();
            }
        }

        public void EndMarbleShot(Component component)
        {
            if (MarbleShotStatus == MarbleShotStatus.PreparingShot)
            {
                MarbleShotStatus = MarbleShotStatus.Shooting;
                cameraController.Zoom(0f);

                lineRenderer = component.GetComponent<LineRenderer>();
                lineRenderer.enabled = false;

                var forceToAdd = (component.transform.position - lastAimRaycastPoint) * ShotPower;
                forceToAdd.y = 0f;

                var componentRigidbody = component.GetComponent<Rigidbody>();

                componentRigidbody.velocity = Vector3.zero;
                componentRigidbody.AddForce(forceToAdd);

                lastEndShot = Time.time;
                battleManager.SetTurn(BattleTurn.Opponent);
            }
        }

        private void HandleAimingOfShot()
        {
            if (MarbleShotStatus == MarbleShotStatus.PreparingShot)
            {
                var ray = Camera.main.ScreenPointToRay(inputManager.MousePosition);
                RaycastHit raycastHit;

                lineRenderer.SetPosition(0, lineRenderer.transform.position);

                ScrollLineRenderer();

                if (Physics.Raycast(ray, out raycastHit, 1000f, layerMask))
                {
                    lastAimRaycastPoint = raycastHit.point;

                    lineRenderer.SetPosition(1, lastAimRaycastPoint);
                }
            }
        }

        private void ScrollLineRenderer()
        {
            var currentScrollPos = lineRenderer.material.GetTextureOffset("_MainTex");

            currentScrollPos.x = currentScrollPos.x + ScrollSpeed * Time.deltaTime;
            lineRenderer.material.SetTextureOffset("_MainTex", currentScrollPos);
        }

        public void FixedTick()
        {
            var player = playerSystem.GetPlayer();

            if (player != null && levelLoader.GetCurrentLevel().GetType() == typeof(BattleGroundLevel))
            {
                HandleShotStatusChangeDueToVelocity(player);
            }
        }

    }
}
