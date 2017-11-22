using Marbles.Components;
using Marbles.Enums;
using Marbles.Systems.Contracts;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class BattleManager : IBattleManager, IFixedTickable, ITickable
    {
        public BattleTurn BattleTurn { get; private set; }

        private readonly IPlayerSystem playerSystem;
        private readonly ICameraController cameraController;
        private readonly IOpponentSystem opponentSystem;

        private Collider lastHitOpponent;

        public BattleManager(IPlayerSystem playerSystem
            , IOpponentSystem opponentSystem
            , ICameraController cameraController)
        {
            this.playerSystem = playerSystem;
            this.opponentSystem = opponentSystem;
            this.cameraController = cameraController;

            BattleTurn = BattleTurn.Player;
        }

        public void Tick()
        {
            if (this.BattleTurn == BattleTurn.Opponent)
            {

            }
        }

        public void FixedTick()
        {
            if (BattleTurn == BattleTurn.Player)
            {
                var player = playerSystem.GetPlayer();

                if (player != null)
                {
                    CheckForOpponentHit(player);
                }
            }
        }

        private void CheckForOpponentHit(GameObject player)
        {
            var collider = player.GetComponent<SphereCollider>();

            var opponentColliders =
                Physics.OverlapSphere(player.transform.position,
                    player.transform.localScale.x * collider.radius + 0.01f)
                .Where(c => c.GetComponent<Opponent>() != null).ToArray();

            if (opponentColliders.Length < 1)
            {
                lastHitOpponent = null;
                return;
            }

            if (lastHitOpponent == null)
            {
                foreach (var opponentCollider in opponentColliders)
                {
                    HandleOpponentHit(opponentCollider);
                }
            }
        }
        
        private void HandleOpponentHit(Collider collider)
        {
            lastHitOpponent = collider;
            Debug.Log("Hit Opponent");
        }

        public void SetTurn(BattleTurn battleTurn)
        {
            this.BattleTurn = battleTurn;

            if (this.BattleTurn == BattleTurn.Player)
            {
                cameraController.Target = playerSystem.GetPlayer().transform;
            }

            if (this.BattleTurn == BattleTurn.Opponent)
            {
                cameraController.Target = opponentSystem.GetOpponent().transform;
            }
        }
    }
}
