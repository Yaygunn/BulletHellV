using BH.Scriptables;
using GH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class UpgradeOption
    {
        public UpgradeType Type { get; }
        public ProjectileType ProjectileType { get; }
        public WeaponUpgradeSO WeaponUpgrade { get; }
        public StatUpgradeSO StatUpgrade { get; }
        public ProjectileDataSO ProjectileData { get; }

        public UpgradeOption(UpgradeType type, ProjectileType projectileType, ProjectileDataSO projectileData = null, 
            WeaponUpgradeSO weaponUpgrade = null, StatUpgradeSO statUpgrade = null)
        {
            Type = type;
            ProjectileType = projectileType;
            ProjectileData = projectileData;
            WeaponUpgrade = weaponUpgrade;
            StatUpgrade = statUpgrade;
        }
    }
}