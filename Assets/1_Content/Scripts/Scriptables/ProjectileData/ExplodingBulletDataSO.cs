using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ExplodingBullet", menuName = "BH/Projectiles/New Exploding Bullet")]
    public class ExplodingBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Exploding Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.ExplodingBullet;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}