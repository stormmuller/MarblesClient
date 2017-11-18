using Marbles.ExtentionMethods;
using Marbles.Systems.Contracts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Systems
{
    public class SystemConfiguration : ISystemConfiguration
    {
        private List<Type> neededComponentTypes = new List<Type>();
        private bool lookUpInParent = true;
        private SystemConfigurationAction systemConfigurationAction;

        public ISystemConfiguration AddType<T>()
        {
            this.neededComponentTypes.Add(typeof(T));
            return this;
        }

        public ISystemConfiguration LookUpInInstanceOnly()
        {
            lookUpInParent = false;
            return this;
        }

        public ISystemConfiguration Calls(SystemConfigurationAction systemConfigurationAction)
        {
            this.systemConfigurationAction = systemConfigurationAction;
            return this;
        }
        
        public bool Handle(Component component)
        {
            var componentTypesFound = new Dictionary<Type, bool>();

            // Add all components into the dictionary and mark them as not found(false).
            neededComponentTypes.ForEach(t => componentTypesFound.Add(t, false));

            foreach (var neededComponentType in neededComponentTypes)
            {
                var matchedComponent = component.GetComponent(neededComponentType);

                if (matchedComponent != null)
                {
                    componentTypesFound[neededComponentType] = true;
                }
            }

            if (!componentTypesFound.AllValuesEqualTo(true))
            {
                foreach (var type in neededComponentTypes)
                {
                    var matchedComponent = component.GetComponentInParent(type);

                    if (matchedComponent != null)
                    {
                        componentTypesFound[type] = true;
                    }
                }
            }

            var isCompliant = componentTypesFound.AllValuesEqualTo(true);

            if (isCompliant)
            {
                systemConfigurationAction(component);
            }

            return isCompliant;
        }
    }
}
