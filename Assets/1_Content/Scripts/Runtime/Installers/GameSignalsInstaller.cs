using BH.Runtime.Audio;
using BH.Runtime.Managers;
using BH.Runtime.Systems;
using Zenject;

namespace BH.Runtime.Installers
{
    public class GameSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            // Audio
            Container.DeclareSignal<AudioStateSignal>();
            
            // Game/Level States
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<LevelStateChangedSignal>();
        }
    }
}