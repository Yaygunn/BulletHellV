using BH.Runtime.Systems;
using BH.Runtime.Test;
using Zenject;

namespace BH.Runtime.Installers
{
    public class GameSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<TestSpawnSignal>();
            Container.DeclareSignal<HealthChangedSignal>();
        }
    }
}