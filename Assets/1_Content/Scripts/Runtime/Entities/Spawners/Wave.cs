using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class Wave
    {
        [field: FoldoutGroup("Spawning"), SerializeField]
        public int MeleeAICount { get; private set; } = 5;
        [field: FoldoutGroup("Spawning"), SerializeField]
        public float MinSpawnInterval { get; private set; } = 1f;
        [field: FoldoutGroup("Spawning"), SerializeField]
        public float MaxSpawnInterval { get; private set; } = 3f;
        [field: FoldoutGroup("Multipliers"), SerializeField]
        public float HealthMultiplier { get; private set; } = 1f;
        [field: FoldoutGroup("Multipliers"), SerializeField]
        public float ShieldMultiplier { get; private set; } = 1f;
        [field: FoldoutGroup("Multipliers"), SerializeField]
        public float DamageMultiplier { get; private set; } = 1f;
        [field: FoldoutGroup("Multipliers"), SerializeField]
        public float SpeedMultiplier { get; private set; } = 1f;
    }
}