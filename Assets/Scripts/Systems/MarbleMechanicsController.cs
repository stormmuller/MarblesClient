using System;
using Marbles.Enums;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;
using Marbles.Components.Levels;
using System.Linq;
using System.Collections.Generic;

namespace Marbles.Systems
{
    public class MarbleMechanicsController : IMarbleMechanicsController, ITickable
    {
        public MarbleShotStatus MarbleShotStatus { get; private set; }
        private float IdleVelocityThresholdSqr { get { return IdleVelocityThreshold * IdleVelocityThreshold; } }
        private float MinDistanceForSuspenseSqr { get { return MinDistanceForSuspense * MinDistanceForSuspense; } }
        private float MinPlayerVelocityForSuspenseSqr { get { return MinPlayerVelocityForSuspense * MinPlayerVelocityForSuspense; } }

        private const string PlaneRayCastLayerName = "Battle Ground Plane";
        private const float ScrollSpeed = 0.75f;
        private const float ShotPower = 15f;
        private const float IdleVelocityThreshold = 0.25f;
        private const float CheckForIdleDelay = 0.5f;
        private const float MinDistanceForSuspense = 0.5f;
        private const float SuspenseTimeScale = 0.05f;
        private const float MinPlayerVelocityForSuspense = 7f;
        private const float StopVelocityMultiplier = 0.3f;

        private readonly IInputManager inputManager;
        private readonly IPlayerSystem playerSystem;
        private readonly IOpponentSystem opponentSystem;
        private readonly ICameraController cameraController;
        private readonly ITimeController timeController;
        private readonly ILevelLoader levelLoader;

        private LineRenderer lineRenderer;
        private LayerMask layerMask;
        private Vector3 lastAimRaycastPoint;
        private float lastEndShot;

        public MarbleMechanicsController(IInputManager inputManager
            , IPlayerSystem playerSystem
            , IOpponentSystem opponentSystem
            , ICameraController cameraController
            , ITimeController timeController
            , ILevelLoader levelLoader)
        {
            this.inputManager = inputManager;
            this.playerSystem = playerSystem;
            this.opponentSystem = opponentSystem;
            this.cameraController = cameraController;
            this.timeController = timeController;
            this.levelLoader = levelLoader;

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
                HandleSuspenseOfEnemyHit(player);
            }
        }

        private void HandleSuspenseOfEnemyHit(GameObject player)
        {
            if (MarbleShotStatus == MarbleShotStatus.Shooting && 
                player.GetComponent<Rigidbody>().velocity.sqrMagnitude > MinPlayerVelocityForSuspenseSqr)
            {
                var playerTransform = player.GetComponent<Transform>();
                var opponents = opponentSystem.GetAllOpponents();

                foreach (var opponent in opponents)
                {
                    var opponentTransform = opponent.GetComponent<Transform>();

                    if ((playerTransform.position - opponentTransform.position).sqrMagnitude < MinDistanceForSuspenseSqr)
                    {
                        cameraController.Zoom(2f, 0.5f);
                        timeController.SetTimeScale(SuspenseTimeScale, 0.5f);
                    }
                }
            }
        }

        private void HandleShotStatusChangeDueToVelocity(GameObject player)
        {
            var playerRigidBody = player.GetComponent<Rigidbody>();

            if (MarbleShotStatus == MarbleShotStatus.Shooting &&
                playerRigidBody.velocity.sqrMagnitude < IdleVelocityThresholdSqr &&
                Time.time - lastEndShot > CheckForIdleDelay)
            {
                var opponents = opponentSystem.GetAllOpponents().Select(o => o.GetComponent<Rigidbody>());
                
                foreach (var opponent in opponents)
                {
                    if (opponent.velocity.sqrMagnitude > IdleVelocityThresholdSqr)
                    {
                        return;
                    }
                }

                var all = new List<Rigidbody>();
                all.AddRange(opponents);
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
    }
}
