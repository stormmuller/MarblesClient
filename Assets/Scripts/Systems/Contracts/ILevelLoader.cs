using Marbles.Components.Levels;

namespace Marbles.Systems.Contracts
{
    public interface ILevelLoader : ISystem
    {
        void LoadLevel(Level level);
        Level GetCurrentLevel();
    }
}
