using Marbles.Components.Levels;
using Marbles.Systems;
using Marbles.Systems.Configurations;
using UnityEngine;
using Zenject;

namespace Marbles.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var mainMenuInstance = (Level)FindObjectOfType<MainMenuLevel>();

            Container
                .BindInstance(mainMenuInstance)
                .WhenInjectedInto<LevelLoader>();

            Container.Bind<LookAtConfiguration>().AsSingle();
            Container.Bind<MouseDownConfiguration>().AsSingle();
            Container.Bind<MouseUpConfiguration>().AsSingle();
            Container.Bind<LevelLoadingConfiguration>().AsSingle();
            Container.Bind<PlayerSystemConfiguration>().AsSingle();

            Container.Bind(x => x.AllInterfaces())
                .To(x => x.AllNonAbstractClasses().InNamespace("Marbles.Systems"))
                .AsSingle();
            
            Container.QueueForInject(mainMenuInstance);
        }
    }
}