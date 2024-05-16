using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ChainReactionEvolution", menuName = "BH/Projectiles/New ChainReaction Evolution")]
    public class ChainReactionProjectileDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Chain Reaction Evolution"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.ChainReactionBullet;
        
        public override ProjectileType GetProjectileType() => Type;
    }
}