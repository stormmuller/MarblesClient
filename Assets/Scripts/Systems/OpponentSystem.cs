using Marbles.Components;
using Marbles.Systems.Contracts;
using System.Linq;
using UnityEngine;

namespace Marbles.Systems
{
    public class OpponentSystem : IOpponentSystem
    {
        public GameObject GetOpponent()
        {
            var opponent = Object.FindObjectOfType<Opponent>();

            return opponent == null ? null : opponent.gameObject;
        }
    }
}
