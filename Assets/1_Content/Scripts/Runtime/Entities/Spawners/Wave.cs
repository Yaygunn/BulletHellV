using System;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class Wave
    {
        [field: SerializeField]
        public int MeleeAICount { get; private set; } = 5;
        [field: SerializeField]
        public float MinSpawnInterval { get; private set; } = 1f;
        [field: SerializeField]
        public float MaxSpawnInterval { get; private set; } = 3f;
    }
}