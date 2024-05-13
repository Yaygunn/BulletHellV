using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "HomingEvolution", menuName = "BH/Projectiles/New Homing Evolution")]
    public class HomingEvolutionDataSO : EvolutionDataSO
    {
        [field: BoxGroup("Homing Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.HomingBullet;
        
        public override ProjectileType GetProjectileType() => Type;
        
        public override void OnEvolve(Projectile projectile)
        {
            Debug.Log("Evolved Homing Bullet");
        }
    }
}