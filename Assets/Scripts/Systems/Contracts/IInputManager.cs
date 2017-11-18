using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface IInputManager : ISystem
    {
        bool LeftMouseButtonDown { get; }
        bool LeftMouseButtonUp { get; }
        Vector3 MousePosition { get; }
    }
}
