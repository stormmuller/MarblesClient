using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;
using System.Collections.Generic;
using Marbles.Systems.Contracts.MarbleMechanics;

namespace Marbles.Systems.Configurations
{
    public class MouseUpConfiguration : IConfigurationHandler
    {
        private readonly List<ISystemConfiguration> Configurations;
        private readonly IHumanMarbleMechanicsController marbleMechanicsController;

        public MouseUpConfiguration(IHumanMarbleMechanicsController marbleMechanicsController)
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
