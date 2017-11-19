using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface ICameraController : ISystem, IRefreshable
    {
        Camera Camera { get; }
        Transform Target { get; set; }
        float ZoomAmount { get; set; }
    }
}
