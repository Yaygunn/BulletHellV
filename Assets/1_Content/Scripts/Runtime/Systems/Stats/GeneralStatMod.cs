using System;
using UnityEngine;

namespace BH.Runtime.Systems
{
    [Serializable]
    public class GeneralStatMod
    {
        [field: SerializeField]
        public int IncreasedHealth { get; set; }
        [field: SerializeField]
        public float HealthMultiplier { get; set; } = 1f;
        [field: SerializeField]
        public int IncreasedShield { get; set; }
        [field: SerializeField]
        public float ShieldMultiplier { get; set; } = 1f;
        [field: SerializeField]
        public float IncreasedSpeed { get; set; }
        [field: SerializeField]
        public float SpeedMultiplier { get; set; } = 1f;
    }
}