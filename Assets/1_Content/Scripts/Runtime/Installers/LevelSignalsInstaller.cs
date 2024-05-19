using BH.Runtime.Entities;
using BH.Runtime.Managers;
using BH.Runtime.UI;
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
            
            // UI
            Container.DeclareSignal<PlayerBulletsChangedSignal>();
            
            // Upgrades
            Container.DeclareSignal<UpgradeSelectedSignal>();
            Container.DeclareSignal<UpgradesShowSignal>();
        }
    }
}