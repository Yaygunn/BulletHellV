using System.Collections.Generic;
using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GH.Scriptables
{
    [CreateAssetMenu(fileName = "StatUpgrade", menuName = "BH/Upgrades/New Stat Upgrade")]
    public class StatUpgradeSO : ScriptableObject
    {
        [field: BoxGroup("General"), SerializeField]
        public string UpgradeName { get; private set; } = "New Upgrade";
        [field: BoxGroup("General"), SerializeField, TextArea]
        public string UpgradeDescription { get; private set; } = "New Upgrade";
        [field: BoxGroup("General"), SerializeField]
        public Sprite Icon { get; private set; }
        
        [field: BoxGroup("Modifications"), SerializeField]
        public int HealthIncrease { get; private set; }

        [field: BoxGroup("Modifications"), SerializeField, MinValue(1f)]
        public float HealthMultiplier { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public int ShieldIncrease { get; private set; }

        [field: BoxGroup("Modifications"), SerializeField, MinValue(1f)]
        public float ShieldMultiplier { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float SpeedIncrease { get; private set; }
        [field: BoxGroup("Modifications"), SerializeField]
        public float SpeedMultiplier { get; private set; }

        public string GetDescription()
        {
            List<string> parts = new ();

            if (HealthIncrease != 0) parts.Add($"Health +{HealthIncrease}");
            if (HealthMultiplier != 0) parts.Add($"Health x{1 + HealthMultiplier:F2}");
            if (ShieldIncrease != 0) parts.Add($"Shield +{ShieldIncrease}");
            if (ShieldMultiplier != 0) parts.Add($"Shield x{1 + ShieldMultiplier:F2}");
            if (SpeedIncrease != 0) parts.Add($"Speed +{SpeedIncrease}");
            if (SpeedMultiplier != 0) parts.Add($"Speed x{1 + SpeedMultiplier:F2}");

            return $"{UpgradeName}: " + string.Join(", ", parts);
        }

        public void ApplyUpgrade(GeneralStatMod mod)
        {
            mod.IncreasedHealth += HealthIncrease;
            mod.HealthMultiplier *= HealthMultiplier;
            mod.IncreasedShield += ShieldIncrease;
            mod.ShieldMultiplier *= ShieldMultiplier;
            mod.IncreasedSpeed += SpeedIncrease;
            mod.SpeedMultiplier *= SpeedMultiplier;
        }
    }
}