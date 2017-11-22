using UnityEngine;
using Zenject;

namespace Marbles.Systems.Contracts
{
    public interface ISuspenseSystem : ISystem, ITickable
    {
        void HandleSuspenseOfEnemyHit(GameObject player);
    }
}
