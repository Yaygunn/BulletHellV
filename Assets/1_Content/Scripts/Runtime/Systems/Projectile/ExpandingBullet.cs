using BH.Scriptables;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class ExpandingBullet : Projectile
    {
        private ExpandingProjectileDataSO _expandingData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is ExpandingProjectileDataSO expandingData)
            {
                _expandingData = expandingData;
            }
            else
            {
                Debug.LogError("[ExpandingBullet] ExpandingProjectileDataSO is not set for ExpandingBullet");
            }
        }

        protected override void HandleObstacleCollision(Collision2D other)
        {
            base.HandleObstacleCollision(other);
            
            _currentSize *= _expandingData.GrowthFactor;
            transform.localScale = Vector3.one * _currentSize;
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
        
        protected override void ResetProperties()
        {
            base.ResetProperties();
            
            _currentSize = _initialSize;
            transform.localScale = Vector3.one * _initialSize;
        }
    }
}