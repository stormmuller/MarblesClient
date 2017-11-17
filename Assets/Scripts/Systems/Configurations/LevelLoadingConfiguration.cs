using System;
using Marbles.Components;
using Marbles.Systems.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Marbles.Components.Levels;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;

namespace Marbles.Systems.Configurations
{
    public class LevelLoadingConfiguration : IConfigurationHandler, ITickable
    {
        private const string WorldMapSceneName = "World Map";
        private readonly List<ISystemConfiguration> Configurations;
        private readonly ILookAtController lookAtController;
        
        private Slider loadingBar;
        private Text percentageText;
        private GameObject rootUIObject;
        private AsyncOperation levelLoadingOperation;

        public LevelLoadingConfiguration(
              GameObject rootUIObject
            , Slider loadingBar
            , Text percentageText
            , ILookAtController lookAtController)
        {
            Configurations = new List<ISystemConfiguration>
            {
                new SystemConfiguration()
                .AddType<WorldMapLevel>()
                .Calls(c => LoadLevel(WorldMapSceneName))
            };
            
            UnityEngine.Object.DontDestroyOnLoad(rootUIObject);

            this.rootUIObject = rootUIObject;
            this.loadingBar = loadingBar;
            this.percentageText = percentageText;

            this.lookAtController = lookAtController;
        }

        public void Handle(Component component)
        {
            foreach (var configuration in Configurations)
            {
                configuration.Handle(component);
            }
        }

        private void LoadLevel(string sceneName)
        {
            this.levelLoadingOperation = SceneManager.LoadSceneAsync(sceneName);
            rootUIObject.SetActive(true);
        }

        public void Tick()
        {
            if (levelLoadingOperation != null)
            {
                if (levelLoadingOperation.isDone)
                {
                    rootUIObject.SetActive(false);
                    lookAtController.Refresh();
                }
                else if (rootUIObject.activeInHierarchy)
                {
                    UpdateLoadingUi(levelLoadingOperation.progress / 0.9f);
                }
            }
        }

        void UpdateLoadingUi(float percentage)
        {
            if (loadingBar != null)
            {
                loadingBar.value = percentage;
            }

            if (percentageText != null)
            {
                percentageText.text = Mathf.Round(percentage * 100) + "%";
            }
        }
    }
}
