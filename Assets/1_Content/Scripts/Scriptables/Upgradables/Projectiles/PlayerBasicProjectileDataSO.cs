﻿using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerBasicProjectile", menuName = "BH/Projectiles/New Player Basic Projectile")]
    public class PlayerBasicProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Player Basic Projectile"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.PlayerBasicBullet;
        [field: BoxGroup("Player Basic Projectile"), SerializeField]
        public float SpeedMultiAfterEvolution { get; private set; } = 0.5f;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}