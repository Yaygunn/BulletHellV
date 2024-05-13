using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HealingEvolution", menuName = "BH/Projectiles/New Healing Evolution")]
    public class HealingEvolutionDataSO : EvolutionDataSO
    {
        [field: BoxGroup("Healing Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HealingBullet;
        
        public override ProjectileType GetProjectileType() => Type;
        
        public override void OnEvolve(Projectile projectile)
        {
            Debug.Log("Evolved into Helaing Bullet");
        }
    }
}