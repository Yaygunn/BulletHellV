using BH.Runtime.Entities;
using Zenject;

namespace BH.Runtime.Installers
{
    public class LevelSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Player
            Container.DeclareSignal<PlayerDiedSignal>();
        }
    }
}