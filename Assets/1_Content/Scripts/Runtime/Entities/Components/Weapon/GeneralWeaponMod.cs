using System;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class GeneralWeaponMod
    {
        [field: SerializeField]
        public float IncreasedDamage { get; set; } = 0;

        [field: SerializeField]
        public float DamageMultiplier { get; set; } = 1;

        [field: SerializeField]
        public float IncreasedProjSpeed { get; set; } = 0;

        [field: SerializeField]
        public float ProjSpeedMultiplier { get; set; } = 1;

        [field: SerializeField]
        public float IncreasedFireRate { get; set; } = 0;

        [field: SerializeField]
        public float FireRateMultiplier { get; set; } = 1;
    }
}