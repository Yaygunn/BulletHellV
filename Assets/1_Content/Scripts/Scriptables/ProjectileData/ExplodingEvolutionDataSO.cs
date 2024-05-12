using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ExplodingEvolution", menuName = "BH/Projectiles/New Exploding Evolution")]
    public class ExplodingEvolutionDataSO : EvolutionDataSO
    {
        [field: BoxGroup("Exploding Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.ExplodingBullet;
        
        public override ProjectileType GetProjectileType() => Type;
        
        public override void OnEvolve(Projectile projectile)
        {
            Debug.Log("Evolved into Exploding Bullet");
        }
    }
}