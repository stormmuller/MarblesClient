using Marbles.Components;
using Marbles.Systems.Contracts;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Systems.Configurations
{
    public class LookAtConfiguration
    {
        public List<ISystemConfiguration> configs = new List<ISystemConfiguration>();

        public LookAtConfiguration()
        {
            configs.Add(new SystemConfiguration<House>().Calls(PerformLookAt));
            configs.Add(new SystemConfiguration<School>().Calls(PerformLookAt));
        }

        private void PerformLookAt(Component lookAt)
        {
            lookAt.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - Camera.main.transform.position);
        }
    }
}
