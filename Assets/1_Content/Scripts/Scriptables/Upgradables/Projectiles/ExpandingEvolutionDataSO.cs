using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ExpandingEvolution", menuName = "BH/Projectiles/New Expanding Evolution")]
    public class ExpandingEvolutionDataSO : EvolutionDataSO
    {
        [field: BoxGroup("Expanding Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.ExpandingBullet;
        
        public override ProjectileType GetProjectileType() => Type;
        
        public override void OnEvolve(Projectile projectile)
        {
            Debug.Log("Evolved Into Expanding Bullet");
        }
    }
}