using Marbles.Components;

namespace Marbles.Systems.Contracts
{
    public interface IHoverableController : ISystem
    {
        void HandleHoverFor(Hoverable hoverable);
        void HandleBlurFor(Hoverable hoveredObject);
    }
}
