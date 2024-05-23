using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ExplodingEvolution", menuName = "BH/Projectiles/New Exploding Evolution")]
    public class ExplodingProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Exploding Evolution"), SerializeField]
        public float ExplosionRadius { get; private set; } = 5f;
        [field: BoxGroup("Exploding Evolution"), SerializeField]
        public float ExplosionForce { get; private set; } = 10f;
        [field: BoxGroup("Exploding Evolution"), SerializeField]
        public bool EffectsOtherProjectiles { get; private set; } = false;
        
        private const ProjectileType _type = ProjectileType.ExplodingBullet;
        
        public override ProjectileType GetProjectileType() => _type;
    }
}