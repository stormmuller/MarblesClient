using Marbles.Components;
using Marbles.Systems.Configurations;
using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class LookAtController : ILookAtController, ITickable
    {
        private LookAt[] lookAtsItems;
        private readonly LookAtConfiguration lookAtConfiguration;

        public LookAtController(LookAtConfiguration lookAtConfiguration)
        {
            this.lookAtConfiguration = lookAtConfiguration;
            Refresh();
        }

        public void Refresh()
        {
            this.lookAtsItems = Object.FindObjectsOfType<LookAt>();
        }

        public void Tick()
        {
            foreach (var lookAtItem in lookAtsItems)
            {
                lookAtConfiguration.Handle(lookAtItem);
            }
        }
    }
}
