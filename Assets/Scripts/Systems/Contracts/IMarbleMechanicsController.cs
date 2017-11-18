using UnityEngine;

namespace Marbles.Systems.Contracts
{
    public interface IMarbleMechanicsController : ISystem
    {
        void PrepareMarbleForShot(Component component);
        void EndMarbleShot(Component component);
    }
}
