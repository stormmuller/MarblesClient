using Marbles.Components.Levels;
using Marbles.Systems.Configurations;
using Marbles.Systems.Contracts;

namespace Marbles.Systems
{
    public class LevelLoader : ILevelLoader
    {
        private readonly LevelLoadingConfiguration levelLoadingConfiguration;

        private Level currentLevel;

        public LevelLoader(LevelLoadingConfiguration levelLoadingConfiguration, Level startingLevel)
        {
            this.levelLoadingConfiguration = levelLoadingConfiguration;
            currentLevel = startingLevel;
        }

        public void LoadLevel(Level level)
        {
            currentLevel = level;
            levelLoadingConfiguration.Handle(level);
        }

        public Level GetCurrentLevel()
        {
            return currentLevel;
        }
    }
}