using Marbles.Components.Levels;
using Marbles.Systems.Configurations;
using Marbles.Systems.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Marbles.Systems
{
    public class LevelLoader : ILevelLoader
    {
        private readonly LevelLoadingConfiguration levelLoadingConfiguration;

        public LevelLoader(LevelLoadingConfiguration levelLoadingConfiguration)
        {
            this.levelLoadingConfiguration = levelLoadingConfiguration;
        }

        public void LoadLevel(Level level)
        {
            levelLoadingConfiguration.Handle(level);
        }
    }
}