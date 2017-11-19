using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface IPlayerSystem : ISystem
    {
        GameObject GetPlayer();
    }
}
