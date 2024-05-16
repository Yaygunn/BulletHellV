using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerBasicProjectile", menuName = "BH/Projectiles/New Player Basic Projectile")]
    public class PlayerBasicProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Player Basic Projectile"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.PlayerBasicBullet;
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}