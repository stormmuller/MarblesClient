using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;
using Marbles.Components.Levels;
using System.Collections.Generic;
using Marbles.Enums;

namespace Marbles.Systems.Configurations
{
    public class MouseDownConfiguration : IConfigurationHandler
    { 
        private readonly List<ISystemConfiguration> Configurations;
        private readonly ILevelLoader levelLoader;
        private readonly IMarbleMechanicsController marbleMechanicsController;

        public MouseDownConfiguration(ILevelLoader levelLoader, IMarbleMechanicsController marbleMechanicsController)
        {
            this.levelLoader = levelLoader;
            this.marbleMechanicsController = marbleMechanicsController;

            this.Configurations = new List<ISystemConfiguration>
            {
                new SystemConfiguration()
                .AddType<Clickable>()
                .AddType<Level>()
                .Calls(c => LoadScene(c)),

                new SystemConfiguration()
                .AddType<Clickable>()
                .AddType<Player>()
                .AddType<Marble>()
                .Calls(c => StartMarbleShot(c))
            };
        }

        public void Handle(Component component)
        {
            Configurations.ForEach(config => config.Handle(component));
        }

        private void LoadScene(Component component)
        {
            this.levelLoader.LoadLevel(component.GetComponent<Level>());
        }

        private void StartMarbleShot(Component component)
        {
            marbleMechanicsController.PrepareMarbleForShot(component);
        }
    }
}
