using Marbles.Components;
using Marbles.Systems.Contracts;
using System.Linq;
using UnityEngine;

namespace Marbles.Systems
{
    public class OpponentSystem : IOpponentSystem
    {
        public GameObject[] GetAllOpponents()
        {
            return Object.
                FindObjectsOfType<Opponent>()
                .Select(o => o.gameObject)
                .ToArray();
        }
    }
}
