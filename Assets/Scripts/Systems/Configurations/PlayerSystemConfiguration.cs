using Marbles.Components;
using Marbles.Systems.Contracts;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Marbles.Systems.Configurations
{
    public class PlayerSystemConfiguration : IConfigurationHandler
    {
        public GameObject Player;

        private readonly List<ISystemConfiguration> configurations;

        public PlayerSystemConfiguration()
        {
            configurations = new List<ISystemConfiguration>
            {
                new SystemConfiguration()
                .AddType<Player>()
                .AddType<Marble>()
                .Calls(c => SetPlayer(c))
            };
        }

        public void Handle(Component component)
        {
            configurations.ForEach(config => config.Handle(component));
        }

        private void SetPlayer(Component component)
        {
            Player = component.gameObject;
        }
    }
}
