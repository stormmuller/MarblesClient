using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface ICameraController : ISystem, IRefreshable
    {
        Camera Camera { get; }
        Transform Target { get; set; }
        void Zoom(float amount);
        void Zoom(float amount, float duration);
    }
}
