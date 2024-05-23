using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "EnemyBasicProjectile", menuName = "BH/Projectiles/New Enemy Basic Projectile")]
    public class EnemyBasicProjectileDataSO : ProjectileDataSO
    {
        private const ProjectileType _type = ProjectileType.EnemyBasicBullet;
        public override ProjectileType GetProjectileType()
        {
            return _type;
        }
    }
}