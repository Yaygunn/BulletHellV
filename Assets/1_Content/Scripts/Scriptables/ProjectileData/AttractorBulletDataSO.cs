using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "AttractorBullet", menuName = "BH/Projectiles/New Attractor Bullet")]
    public class AttractorBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Attractor Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.AttractorBullet;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}