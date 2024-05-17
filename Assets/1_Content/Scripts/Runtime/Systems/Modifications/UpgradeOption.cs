using GH.Scriptables;

namespace BH.Runtime.Systems
{
    public class UpgradeOption
    {
        public UpgradeType Type { get; }
        public ProjectileType ProjectileType { get; }
        public string Description { get; }
        public WeaponUpgradeSO WeaponUpgrade { get; }
        public StatUpgradeSO StatUpgrade { get; }

        public UpgradeOption(UpgradeType type, ProjectileType projectileType, string description, WeaponUpgradeSO weaponUpgrade = null, StatUpgradeSO statUpgrade = null)
        {
            Type = type;
            ProjectileType = projectileType;
            Description = description;
            WeaponUpgrade = weaponUpgrade;
            StatUpgrade = statUpgrade;
        }
    }
}