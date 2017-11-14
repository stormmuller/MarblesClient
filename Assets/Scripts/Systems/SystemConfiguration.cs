using Marbles.Components;
using Marbles.Systems.Contracts;
using System;
using UnityEngine;

namespace Marbles.Systems
{
    public class SystemConfiguration<T> : ISystemConfiguration<T> where T : Component, IComponent
    {
        private Type type;
        private bool lookUpInParent = true;
        private SystemConfigurationAction action;

        public SystemConfiguration()
        {
            this.type = typeof(T);
        }

        public ISystemConfiguration<T> LookUpInInstanceOnly()
        {
            lookUpInParent = false;
            return this;
        }

        public ISystemConfiguration<T> Calls(SystemConfigurationAction action)
        {
            this.action = action;
            return this;
        }

        public bool Handle(Component component)
        {
            if (component.GetComponent<T>() != null)
            {
                action(component);
                return true;
            }
            else if (lookUpInParent && component.GetComponentInParent<T>() != null)
            {
                action(component);
                return true;
            }
            
            return false;
        }
    }
}
