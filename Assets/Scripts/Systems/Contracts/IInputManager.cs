using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface IInputManager : ISystem
    {
        bool LeftMouseButtonPress { get; }
        Vector3 MousePosition { get; }
    }
}
