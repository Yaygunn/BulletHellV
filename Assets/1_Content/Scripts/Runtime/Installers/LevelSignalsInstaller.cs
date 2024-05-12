using BH.Runtime.Entities;
using BH.Scripts.Runtime.UI;
using Zenject;

namespace BH.Runtime.Installers
{
    public class LevelSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Player
            Container.DeclareSignal<PlayerDiedSignal>();
            
            // Upgrades
            Container.DeclareSignal<UpgradeSelectedSignal>();
            Container.DeclareSignal<UpgradesShowSignal>();
        }
    }
}