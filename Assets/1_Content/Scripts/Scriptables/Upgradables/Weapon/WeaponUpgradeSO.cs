using System.Collections.Generic;
using BH.Runtime.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GH.Scriptables
{
    [CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "BH/Upgrades/New Weapon Upgrade")]
    public class WeaponUpgradeSO : ScriptableObject
    {
        [field: BoxGroup("General"), SerializeField]
        public string UpgradeName { get; private set; } = "New Upgrade";
        
        [field: BoxGroup("Modifications"), SerializeField]
        public float DamageIncrease { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float DamageMultiplier { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float SpeedIncrease { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float SpeedMultiplier { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float FireRateIncrease { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float fireRateMultiplier { get; private set; }

        public string GetDescription()
        {
            List<string> parts = new ();

            if (DamageIncrease != 0) parts.Add($"Damage +{DamageIncrease}");
            if (DamageMultiplier != 0) parts.Add($"Damage x{1 + DamageMultiplier:F2}");
            if (SpeedIncrease != 0) parts.Add($"Speed +{SpeedIncrease}");
            if (SpeedMultiplier != 0) parts.Add($"Speed x{1 + SpeedMultiplier:F2}");
            if (FireRateIncrease != 0) parts.Add($"Fire Rate +{FireRateIncrease}");
            if (fireRateMultiplier != 0) parts.Add($"Fire Rate x{1 + fireRateMultiplier:F2}");

            return $"{UpgradeName}: " + string.Join(", ", parts);
        }

        public void ApplyUpgrade(GeneralWeaponMod mod)
        {
            mod.IncreasedDamage += DamageIncrease;
            mod.DamageMultiplier *= (1 + DamageMultiplier);
            mod.IncreasedProjSpeed += SpeedIncrease;
            mod.ProjSpeedMultiplier *= (1 + SpeedMultiplier);
            mod.IncreasedFireRate += FireRateIncrease;
            mod.FireRateMultiplier *= (1 + fireRateMultiplier);
        }
    }
}