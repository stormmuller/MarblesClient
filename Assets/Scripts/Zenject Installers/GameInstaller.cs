using Marbles.Systems.Configurations;
using Zenject;

namespace Marbles.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LookAtConfiguration>().AsSingle();
            Container.Bind<MouseDownConfiguration>().AsSingle();
            Container.Bind<MouseUpConfiguration>().AsSingle();
            Container.Bind<LevelLoadingConfiguration>().AsSingle();

            Container.Bind(x => x.AllInterfaces())
                .To(x => x.AllNonAbstractClasses().InNamespace("Marbles.Systems"))
                .AsSingle();
        }
    }
}