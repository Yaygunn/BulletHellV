using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "AttractorEvolution", menuName = "BH/Projectiles/New Attractor Evolution")]
    public class AttractorEvolutionDataSO : EvolutionDataSO
    {
        [field: BoxGroup("Attractor Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.AttractorBullet;
        
        public override ProjectileType GetProjectileType() => Type;
        
        public override void OnEvolve(Projectile projectile)
        {
            Debug.Log("Evolved Into Attractor Bullet");
        }
    }
}