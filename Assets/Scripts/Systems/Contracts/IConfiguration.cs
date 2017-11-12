using Marbles.Components;

namespace Marbles.Systems.Contracts
{
    public interface IConfiguration<T> where T : IComponent
    {
        bool IsEntityCompliant(T component);
        bool IsEntityOrParentCompliant(T component);
    }
}
