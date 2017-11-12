using Marbles.Components;
using Marbles.Systems.Contracts;
using Marbles.Systems.LookAtConfiguration;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class LookAtController : ILookAtController, ITickable
    {
        private readonly LookAt[] lookAtsItems;
        private readonly List<ILookAtConfiguration> lookAtConfigurations;

        public LookAtController(List<ILookAtConfiguration> lookAtConfigurations)
        {
            this.lookAtsItems = Object.FindObjectsOfType<LookAt>();
            this.lookAtConfigurations = lookAtConfigurations;
        }

        public void Tick()
        {
            foreach (var lookAtConfiguration in lookAtConfigurations)
            {
                foreach (var lookAtItem in lookAtsItems)
                {
                    if (lookAtConfiguration.IsEntityOrParentCompliant(lookAtItem))
                    {
                        lookAtConfiguration.PerformLookUp(lookAtItem);
                    }
                }
            }
        }
    }
}
