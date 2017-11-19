using Marbles.Systems.Contracts;
using UnityEngine;
using UnityEngine.UI;
using Marbles.Components.Levels;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;
using System;

namespace Marbles.Systems.Configurations
{
    public class LevelLoadingConfiguration : IConfigurationHandler, ITickable
    {
        private const string WorldMapSceneName = "World Map";
        private const string BattleGroundSceneName = "Battle Ground";

        private readonly List<ISystemConfiguration> Configurations;
        private readonly ILookAtController lookAtController;
        private readonly ICameraController cameraController;
        private readonly IPlayerSystem playerSystem;
        
        private Slider loadingBar;
        private Text percentageText;
        private GameObject rootUIObject;
        private AsyncOperation levelLoadingOperation;
        private string sceneCurrentlyLoading;

        public LevelLoadingConfiguration(
              GameObject rootUIObject
            , Slider loadingBar
            , Text percentageText
            , ILookAtController lookAtController
            , ICameraController cameraController
            , IPlayerSystem playerSystem)
        {
            Configurations = new List<ISystemConfiguration>
            {
                new SystemConfiguration()
                .AddType<WorldMapLevel>()
                .Calls(c => LoadLevel(WorldMapSceneName)),

                new SystemConfiguration()
                .AddType<BattleGroundLevel>()
                .Calls(c => LoadLevel(BattleGroundSceneName))
            };
            
            UnityEngine.Object.DontDestroyOnLoad(rootUIObject);

            this.rootUIObject = rootUIObject;
            this.loadingBar = loadingBar;
            this.percentageText = percentageText;

            this.lookAtController = lookAtController;
            this.cameraController = cameraController;
            this.playerSystem = playerSystem;
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
            sceneCurrentlyLoading = sceneName;

            this.levelLoadingOperation = SceneManager.LoadSceneAsync(sceneName);
            rootUIObject.SetActive(true);
        }

        private void ResetCamrea(string sceneName)
        {
            if (sceneName == BattleGroundSceneName)
            {
                cameraController.Target = playerSystem.GetPlayer().transform;
                cameraController.Refresh();
            }
        }

        public void Tick()
        {
            if (levelLoadingOperation != null)
            {
                if (levelLoadingOperation.isDone)
                {
                    rootUIObject.SetActive(false);
                    lookAtController.Refresh();
                    ResetCamrea(sceneCurrentlyLoading);

                    sceneCurrentlyLoading = null;
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
