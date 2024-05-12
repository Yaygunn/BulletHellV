using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HomingBullet", menuName = "BH/Projectiles/New Homing Bullet")]
    public class HomingBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Homing Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HomingBullet;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}