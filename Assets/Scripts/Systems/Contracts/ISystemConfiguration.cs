using Marbles.Components;
using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface ISystemConfiguration<T> : ISystemConfiguration where T : Component, IComponent
    {
        ISystemConfiguration<T> LookUpInInstanceOnly();
        ISystemConfiguration<T> Calls(SystemConfigurationAction action);
    }

    public interface ISystemConfiguration
    {
        bool Handle(Component component);
    }

    public delegate void SystemConfigurationAction(Component component);
}
