using Zenject;

namespace Marbles.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(x => x.AllInterfaces())
                .To(x => x.AllNonAbstractClasses().InNamespace("Marbles.Systems"))
                .AsSingle();
        }
    }
}