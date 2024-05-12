using System.Collections.Generic;
using BH.Runtime.Systems;

namespace BH.Scripts.Runtime.UI
{
    public struct UpgradesShowSignal
    {
        public List<UpgradeOption> UpgradeOptions { get; }
        
        public UpgradesShowSignal(List<UpgradeOption> upgradeOptions)
        {
            UpgradeOptions = upgradeOptions;
        }
    }
}