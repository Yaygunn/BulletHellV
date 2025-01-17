﻿using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ExpandingEvolution", menuName = "BH/Projectiles/New Expanding Evolution")]
    public class ExpandingProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Expanding Evolution"), SerializeField]
        public float GrowthFactor { get; private set; } = 2f;
        
        private const ProjectileType _type = ProjectileType.ExpandingBullet;
        
        public override ProjectileType GetProjectileType() => _type;
    }
}