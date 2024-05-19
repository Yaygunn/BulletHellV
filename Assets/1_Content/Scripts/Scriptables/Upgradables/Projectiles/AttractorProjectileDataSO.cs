using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "AttractorEvolution", menuName = "BH/Projectiles/New Attractor Evolution")]
    public class AttractorProjectileDataSO : ProjectileDataSO
    {
        
        [field: BoxGroup("Attractor Evolution"), SerializeField]
        public float AttractionRadius { get; private set; } = 5f;
        [field: BoxGroup("Attractor Evolution"), SerializeField]
        public float MinDistanceThreshold { get; private set; } = 1f;
        [field: BoxGroup("Attractor Evolution"), SerializeField]
        public float AttractionForce { get; private set; } = 0.2f;
        [field: BoxGroup("Attractor Evolution"), SerializeField]
        public float AttractionDuration { get; private set; } = 5f;

        private const ProjectileType _type = ProjectileType.AttractorBullet;

        public override ProjectileType GetProjectileType() => _type;
    }
}