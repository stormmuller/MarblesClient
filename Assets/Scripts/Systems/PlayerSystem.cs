using Marbles.Components;
using Marbles.Systems.Configurations;
using Marbles.Systems.Contracts;
using UnityEngine;

namespace Marbles.Systems
{
    public class PlayerSystem : IPlayerSystem
    {
        private readonly PlayerSystemConfiguration configuration;

        public PlayerSystem(PlayerSystemConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public GameObject GetPlayer()
        {
            var allPlayers = Object.FindObjectsOfType<Player>();

            foreach (var player in allPlayers)
            {
                configuration.Handle(player);
            }

            return configuration.Player;
        }
    }
}
