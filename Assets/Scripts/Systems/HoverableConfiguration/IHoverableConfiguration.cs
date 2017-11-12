using Marbles.Components;
using Marbles.Systems.Contracts;

namespace Marbles.Systems.HoverableConfiguration
{
    public interface IHoverableConfiguration : IConfiguration<Hoverable>
    {
        void OnHover(Hoverable hoverable);
        void OnBlur(Hoverable hoverable);
    }
}
