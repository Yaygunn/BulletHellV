using BH.Runtime.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Scriptables
{
    [CreateAssetMenu(fileName = "ChainReactionBullet", menuName = "BH/Projectiles/New ChainReaction Bullet")]
    public class ChainReactionBulletDataSO : ProjectileDataSO
    {
        [field: BoxGroup("Chain Reaction Bullet"), SerializeField, ReadOnly]
        public ProjectileType Type { get; private set; } = ProjectileType.ChainReactionBullet;
        
        public override ProjectileType GetProjectileType()
        {
            return Type;
        }
    }
}