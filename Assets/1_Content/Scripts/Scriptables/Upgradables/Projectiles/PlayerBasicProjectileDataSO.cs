using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerBasicProjectile", menuName = "BH/Projectiles/New Player Basic Projectile")]
    public class PlayerBasicProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Player Basic Projectile"), SerializeField]
        public float SpeedMultiAfterEvolution { get; private set; } = 0.5f;
        
        private const ProjectileType _type = ProjectileType.PlayerBasicBullet;
        
        public override ProjectileType GetProjectileType() => _type;
    }
}