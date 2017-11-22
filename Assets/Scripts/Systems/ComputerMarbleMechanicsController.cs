using System;
using Marbles.Components.Levels;
using Marbles.Enums;
using Marbles.Systems.Contracts;
using Marbles.Systems.Contracts.MarbleMechanics;
using UnityEngine;
using Marbles.Components;

namespace Marbles.Systems.HoverableConfiguration
{
    public class ComputerMarbleMechanicsController : IComputerMarbleMechanicsController
    {
        private const float ScrollSpeed = 0.75f;

        private readonly IBattleManager battleManager;
        private readonly IOpponentSystem opponentSystem;
        private readonly IPlayerSystem playerSystem;
        private readonly ILevelLoader levelLoader;

        private LineRenderer lineRenderer;
        private Vector3 flipVectorMultiplier = new Vector3(-1, 0, -1);

        public MarbleShotStatus MarbleShotStatus { get; private set; }
        
        public ComputerMarbleMechanicsController(IBattleManager battleManager
            , IPlayerSystem playerSystem
            , IOpponentSystem opponentSystem
            , ILevelLoader levelLoader)
        {
            this.battleManager = battleManager;
            this.opponentSystem = opponentSystem;
            this.levelLoader = levelLoader;
            this.playerSystem = playerSystem;
        }

        public void Tick()
        {
            var opponent = opponentSystem.GetOpponent();

            if (battleManager.BattleTurn == BattleTurn.Opponent && MarbleShotStatus == MarbleShotStatus.Idle)
            {
                PrepareMarbleForShot(opponent.GetComponent<Opponent>());
            }

            if (levelLoader.GetCurrentLevel().GetType() == typeof(BattleGroundLevel) && opponent != null)
            {
                HandleAimingOfShot();
                // HandleShotStatusChangeDueToVelocity(opponent);
            }
        }

        private void HandleShotStatusChangeDueToVelocity(GameObject opponent)
        {
            throw new NotImplementedException();
        }

        private void HandleAimingOfShot()
        {
            if (MarbleShotStatus == MarbleShotStatus.PreparingShot)
            {
                ScrollLineRenderer();

                var opponentPosition = lineRenderer.transform.position;
                var playerPosition = playerSystem.GetPlayer().transform.position;

                var localPlayerVector = playerPosition - opponentPosition;
                var localReflectionVector = Vector3.Scale(localPlayerVector, flipVectorMultiplier);
                var reflectionVector = localReflectionVector + opponentPosition;

                lineRenderer.SetPosition(0, opponentPosition);
                lineRenderer.SetPosition(1, reflectionVector);
            }
        }

        private void ScrollLineRenderer()
        {
            var currentScrollPos = lineRenderer.material.GetTextureOffset("_MainTex");

            currentScrollPos.x = currentScrollPos.x + ScrollSpeed * Time.deltaTime;
            lineRenderer.material.SetTextureOffset("_MainTex", currentScrollPos);
        }

        public void EndMarbleShot(Component component)
        {
            throw new System.NotImplementedException();
        }

        public void PrepareMarbleForShot(Component component)
        {
            if (MarbleShotStatus == MarbleShotStatus.Idle)
            {
                MarbleShotStatus = MarbleShotStatus.PreparingShot;

                lineRenderer = component.GetComponent<LineRenderer>();
                lineRenderer.enabled = true;

                HandleAimingOfShot();
            }
        }
    }
}
