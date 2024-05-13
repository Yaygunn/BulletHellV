using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class HealingBullet : Projectile
    {
        [BoxGroup("Healing Bullet"), SerializeField]
        private int _healAmount = 25;
        
        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(-_healAmount);
                ReturnToPool();
                return;
            }
            
            base.OnCollisionEnter2D(other);
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}