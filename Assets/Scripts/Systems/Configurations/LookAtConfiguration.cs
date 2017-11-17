using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;

namespace Marbles.Systems.Configurations
{
    public class LookAtConfiguration : IConfigurationHandler
    {
        public readonly ISystemConfiguration Configuration;

        public LookAtConfiguration()
        {
            Configuration = new SystemConfiguration()
                .AddType<House>()
                .AddType<School>()
                .Calls(c => PerformLookAt(c.transform));
        }

        public void Handle(Component component)
        {
            Configuration.Handle(component);
        }

        private void PerformLookAt(Transform lookAt)
        {
            lookAt.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - Camera.main.transform.position);
        }
    }
}
