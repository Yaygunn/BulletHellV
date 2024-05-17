﻿using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HomingEvolution", menuName = "BH/Projectiles/New Homing Evolution")]
    public class HomingProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Homing Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HomingBullet;
        [field: BoxGroup("Homing Evolution"), SerializeField]
        public float HomingStrength { get; private set; } = 0.1f;
        [field: BoxGroup("Homing Evolution"), SerializeField]
        public float TargetDetectionRadius { get; private set; } = 10f;
        [field: BoxGroup("Homing Evolution"), SerializeField]
        public float TargetCheckInterval { get; private set; } = 1f;
        
        public override ProjectileType GetProjectileType() => Type;
    }
}