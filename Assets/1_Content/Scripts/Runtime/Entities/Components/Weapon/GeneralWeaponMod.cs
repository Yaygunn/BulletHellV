using System;
using UnityEngine;

namespace BH.Runtime.Entities
{
    [Serializable]
    public class GeneralWeaponMod
    {
        [field: SerializeField]
        public float IncreasedDamage { get; private set; }
        [field: SerializeField]
        public float DamageMultiplier { get; private set; }
        [field: SerializeField]
        public float IncreasedProjSpeed { get; private set; }
        [field: SerializeField]
        public float ProjSpeedMultiplier { get; private set; }
        [field: SerializeField]
        public float IncreasedFireRate { get; private set; }
        [field: SerializeField]
        public float FireRateMultiplier { get; private set; }
    }
}