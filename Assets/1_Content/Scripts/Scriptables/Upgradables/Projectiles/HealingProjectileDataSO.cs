using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HealingEvolution", menuName = "BH/Projectiles/New Healing Evolution")]
    public class HealingProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Healing Evolution"), SerializeField]
        public int HealAmount { get; private set; } = 25;
        
        private const ProjectileType _type = ProjectileType.HealingBullet;
        
        public override ProjectileType GetProjectileType() => _type;
    }
}