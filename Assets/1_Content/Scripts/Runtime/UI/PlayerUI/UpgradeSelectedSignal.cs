namespace BH.Scripts.Runtime.UI
{
    public struct UpgradeSelectedSignal
    {
        public int UpgradeIndex { get; }
        
        public UpgradeSelectedSignal(int upgradeIndex)
        {
            UpgradeIndex = upgradeIndex;
        }
    }
}