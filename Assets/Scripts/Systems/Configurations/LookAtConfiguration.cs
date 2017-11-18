using Marbles.Components;
using Marbles.Systems.Contracts;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Systems.Configurations
{
    public class LookAtConfiguration : IConfigurationHandler
    {
        public readonly List<ISystemConfiguration> Configurations;

        public LookAtConfiguration()
        {
            Configurations = new List<ISystemConfiguration>()
            {
                new SystemConfiguration()
                .AddType<House>()
                .Calls(c => PerformLookAt(c.transform)),

                new SystemConfiguration()
                .AddType<School>()
                .Calls(c => PerformLookAt(c.transform))
            };
        }

        public void Handle(Component component)
        {
            Configurations.ForEach(config => config.Handle(component));
        }

        private void PerformLookAt(Transform lookAt)
        {
            lookAt.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - Camera.main.transform.position);
        }
    }
}
