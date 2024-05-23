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
        [field: BoxGroup("General"), SerializeField, TextArea]
        public string UpgradeDescription { get; private set; } = "New Upgrade";
        [field: BoxGroup("General"), SerializeField]
        public Sprite Icon { get; private set; }
        
        [field: BoxGroup("Modifications"), SerializeField]
        public int DamageIncrease { get; private set; }

        [field: BoxGroup("Modifications"), SerializeField, MinValue(1f)]
        public float DamageMultiplier { get; private set; } = 1f;
        [field: BoxGroup("Modifications"), SerializeField]
        public float ProjSpeedIncrease { get; private set; }

        [field: BoxGroup("Modifications"), SerializeField, MinValue(1f)]
        public float ProjSpeedMultiplier { get; private set; } = 1f;
        [field: BoxGroup("Modifications"), SerializeField]
        public float FireRateIncrease { get; private set; }

        [field: BoxGroup("Modifications"), SerializeField]
        public float FireRateMultiplier { get; private set; } = 1f;

        public string GetDescription()
        {
            List<string> parts = new ();

            if (DamageIncrease != 0) parts.Add($"Damage +{DamageIncrease}");
            if (DamageMultiplier != 0) parts.Add($"Damage x{1 + DamageMultiplier:F2}");
            if (ProjSpeedIncrease != 0) parts.Add($"Speed +{ProjSpeedIncrease}");
            if (ProjSpeedMultiplier != 0) parts.Add($"Speed x{1 + ProjSpeedMultiplier:F2}");
            if (FireRateIncrease != 0) parts.Add($"Fire Rate +{FireRateIncrease}");
            if (FireRateMultiplier != 0) parts.Add($"Fire Rate x{1 + FireRateMultiplier:F2}");

            return $"{UpgradeName}: " + string.Join(", ", parts);
        }

        public void ApplyUpgrade(GeneralWeaponMod mod)
        {
            mod.IncreasedDamage += DamageIncrease;
            mod.DamageMultiplier *= DamageMultiplier;
            mod.IncreasedProjSpeed += ProjSpeedIncrease;
            mod.ProjSpeedMultiplier *= ProjSpeedMultiplier;
            mod.IncreasedFireRate += FireRateIncrease;
            mod.FireRateMultiplier *= FireRateMultiplier;
        }
    }
}