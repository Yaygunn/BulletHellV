using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HealingEvolution", menuName = "BH/Projectiles/New Healing Evolution")]
    public class HealingProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Healing Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HealingBullet;
        [field: BoxGroup("Healing Evolution"), SerializeField]
        public int HealAmount { get; private set; } = 25;
        
        public override ProjectileType GetProjectileType() => Type;
    }
}