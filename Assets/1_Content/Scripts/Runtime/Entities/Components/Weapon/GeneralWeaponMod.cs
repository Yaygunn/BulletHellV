using System;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class GeneralWeaponMod
    {
        [field: SerializeField]
        public float IncreasedDamage { get; private set; } = 0;

        [field: SerializeField]
        public float DamageMultiplier { get; private set; } = 1;

        [field: SerializeField]
        public float IncreasedProjSpeed { get; private set; } = 0;

        [field: SerializeField]
        public float ProjSpeedMultiplier { get; private set; } = 1;

        [field: SerializeField]
        public float IncreasedFireRate { get; private set; } = 0;

        [field: SerializeField]
        public float FireRateMultiplier { get; private set; } = 1;
    }
}