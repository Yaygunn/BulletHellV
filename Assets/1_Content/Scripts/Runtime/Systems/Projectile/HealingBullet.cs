using BH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class HealingBullet : Projectile
    {
        private HealingProjectileDataSO _healingData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is HealingProjectileDataSO healingData)
            {
                _healingData = healingData;
            }
            else
            {
                Debug.LogError("[HealingBullet] HealingProjectileDataSO is not set for HealingBullet");
            }
        }
        
        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.HandleDamage(-_healingData.HealAmount);
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