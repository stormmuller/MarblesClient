using Marbles.Components;
using Marbles.Systems.Contracts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Marbles.Systems
{
    public class SystemConfiguration : ISystemConfiguration
    {
        private List<Type> types = new List<Type>();
        private bool lookUpInParent = true;
        private SystemConfigurationAction systemConfigurationAction;

        public SystemConfiguration()
        {

        }

        public ISystemConfiguration AddType<T>()
        {
            this.types.Add(typeof(T));
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
            Component matchedComponent;
            return Handle(component, out matchedComponent);
        }
        
        public bool Handle(Component component, out Component matchedComponent)
        {
            foreach (var type in types)
            {
                var _matchedComponent = component.GetComponent(type);

                if (_matchedComponent != null)
                {
                    matchedComponent = _matchedComponent;
                    systemConfigurationAction(component);
                    return true;
                }
            }

            foreach (var type in types)
            {
                var _matchedComponent = component.GetComponent(type);

                if (lookUpInParent && component.GetComponentInParent(type) != null)
                {
                    matchedComponent = _matchedComponent;
                    systemConfigurationAction(component);
                    return true;
                }
            }

            matchedComponent = null;
            return false;
        }
    }
}
