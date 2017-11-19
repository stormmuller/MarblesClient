using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;
using Marbles.Components.Levels;
using System.Collections.Generic;
using System;

namespace Marbles.Systems.Configurations
{
    public class MouseUpConfiguration : IConfigurationHandler
    {
        private readonly List<ISystemConfiguration> Configurations;
        private readonly IMarbleMechanicsController marbleMechanicsController;

        public MouseUpConfiguration(IMarbleMechanicsController marbleMechanicsController)
        {
            this.marbleMechanicsController = marbleMechanicsController;

            this.Configurations = new List<ISystemConfiguration>
            {
                new SystemConfiguration()
                .AddType<Clickable>()
                .AddType<Player>()
                .AddType<Marble>()
                .Calls(c => EndShootMarble(c))
            };
        }

        public void Handle(Component component)
        {
            Configurations.ForEach(config => config.Handle(component));
        }

        private void EndShootMarble(Component component)
        {
            marbleMechanicsController.EndMarbleShot(component);
        }
    }
}
