using Marbles.Components;
using Marbles.Systems.Contracts;

namespace Marbles.Systems.LookAtConfiguration
{
    public interface ILookAtConfiguration : IConfiguration<LookAt>
    {
        void PerformLookUp(LookAt lookAt);
    }
}
