using Marbles.Components;
using System;
using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface ISystemConfiguration
    {
        ISystemConfiguration LookUpInInstanceOnly();
        ISystemConfiguration Calls(SystemConfigurationAction action);
        ISystemConfiguration AddType<T>();
        bool Handle(Component component);
    }

    public delegate void SystemConfigurationAction(Component component);
}
