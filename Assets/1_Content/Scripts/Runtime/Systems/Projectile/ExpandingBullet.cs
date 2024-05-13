using UnityEngine;

namespace BH.Runtime.Systems
{
    public class ExpandingBullet : Projectile
    {
        [SerializeField]
        private float _growthFactor = 1.2f;
        
        protected override void HandleObstacleCollision(Collision2D other)
        {
            base.HandleObstacleCollision(other);
            
            _currentSize *= _growthFactor;
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