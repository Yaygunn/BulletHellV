using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class ChainReactionBullet : Projectile
    {
        // TODO: Implement this when we have enemies..

        private ChainReactionProjectileDataSO _chainReactionData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is ChainReactionProjectileDataSO chainReactionData)
            {
                _chainReactionData = chainReactionData;
            }
            else
            {
                Debug.LogError("[ChainReactionBullet] ChainReactionProjectileDataSo is not set for ChainReactionBullet");
            }
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}