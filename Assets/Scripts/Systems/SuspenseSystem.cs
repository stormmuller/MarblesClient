using Marbles.Components.Levels;
using Marbles.Enums;
using Marbles.Systems.Contracts;
using UnityEngine;

namespace
    Marbles.Systems
{
    public class SuspenseSystem : ISuspenseSystem
    {
        private const float MinDistanceForSuspense = 0.5f;
        private const float MinPlayerVelocityForSuspense = 7f;
        private const float SuspenseTimeScale = 0.05f;

        private readonly IOpponentSystem opponentSystem;
        private readonly ICameraController cameraController;
        private readonly ITimeController timeController;
        private readonly IPlayerSystem playerSystem;
        private readonly ILevelLoader levelLoader;

        private float MinPlayerVelocityForSuspenseSqr { get { return MinPlayerVelocityForSuspense * MinPlayerVelocityForSuspense; } }
        private float MinDistanceForSuspenseSqr { get { return MinDistanceForSuspense * MinDistanceForSuspense; } }

        public SuspenseSystem(IPlayerSystem playerSystem
            , IOpponentSystem opponentSystem
            , ICameraController cameraController
            , ITimeController timeController
            , ILevelLoader levelLoader)
        {
            this.opponentSystem = opponentSystem;
            this.cameraController = cameraController;
            this.timeController = timeController;
            this.playerSystem = playerSystem;
            this.levelLoader = levelLoader;
        }

        public void HandleSuspenseOfEnemyHit(GameObject player)
        {
            if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > MinPlayerVelocityForSuspenseSqr)
            {
                var playerTransform = player.GetComponent<Transform>();
                var opponent = opponentSystem.GetOpponent();

                var opponentTransform = opponent.GetComponent<Transform>();

                if ((playerTransform.position - opponentTransform.position).sqrMagnitude < MinDistanceForSuspenseSqr)
                {
                    cameraController.Zoom(2f, 0.5f);
                    timeController.SetTimeScale(SuspenseTimeScale, 0.5f);
                }
            }
        }

        public void Tick()
        {
            var player = playerSystem.GetPlayer();

            if (levelLoader.GetCurrentLevel().GetType() == typeof(BattleGroundLevel) && player != null)
            {
                HandleSuspenseOfEnemyHit(player);
            }
        }
    }
}
