using Marbles.Systems.Configurations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace Marbles.Installers
{
    public class ConfigHandlerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var rootUIObject = GameObject.Find("Level Loader");
            var loadingBar = GameObject.Find("Loading Bar").GetComponent<Slider>();
            var percentageText = GameObject.Find("Percentage Text").GetComponent<Text>();

            Container
                .BindInstance(rootUIObject)
                .WhenInjectedInto<LevelLoadingConfiguration>();

            Container
                .BindInstance(loadingBar)
                .WhenInjectedInto<LevelLoadingConfiguration>();

            Container
                .BindInstance(percentageText)
                .WhenInjectedInto<LevelLoadingConfiguration>();

            Container.InjectGameObject(rootUIObject);
            Container.QueueForInject(loadingBar);
            Container.QueueForInject(percentageText);
        }
    }
}
