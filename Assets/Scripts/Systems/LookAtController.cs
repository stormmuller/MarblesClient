using Marbles.Components;
using Marbles.Systems.Configurations;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class LookAtController : ILookAtController, ITickable
    {
        private readonly LookAt[] lookAtsItems;
        private readonly LookAtConfiguration lookAtConfiguration;

        public LookAtController(LookAtConfiguration lookAtConfiguration)
        {
            this.lookAtsItems = Object.FindObjectsOfType<LookAt>();
            this.lookAtConfiguration = lookAtConfiguration;
        }

        public void Tick()
        {
            foreach (var config in lookAtConfiguration.configs)
            {
                foreach (var lookAtItem in lookAtsItems)
                {
                    config.Handle(lookAtItem);
                }
            }
        }
    }
}
