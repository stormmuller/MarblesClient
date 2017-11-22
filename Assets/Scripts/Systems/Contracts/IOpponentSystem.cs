using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface IOpponentSystem : ISystem
    {
        GameObject GetOpponent();
    }
}
