using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "EnemyBasicProjectile", menuName = "BH/Projectiles/New Enemy Basic Projectile")]
    public class EnemyBasicProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Enemy Basic Projectile"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.EnemyBasicBullet;
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}