using Marbles.Enums;
using UnityEngine;
using Zenject;

namespace Marbles.Systems.Contracts.MarbleMechanics
{
    public interface IMarbleMechanicsController : ISystem, ITickable
    {
        void PrepareMarbleForShot(Component component);
        void EndMarbleShot(Component component);
        MarbleShotStatus MarbleShotStatus { get; }
    }
}
