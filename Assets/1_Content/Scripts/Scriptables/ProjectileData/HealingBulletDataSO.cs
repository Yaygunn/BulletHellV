using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HealingBullet", menuName = "BH/Projectiles/New Healing Bullet")]
    public class HealingBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Healing Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HealingBullet;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}