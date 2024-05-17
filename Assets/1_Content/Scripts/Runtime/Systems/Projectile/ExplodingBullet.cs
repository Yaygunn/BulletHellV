using System.Collections.Generic;
using BH.Scriptables;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class ExplodingBullet : Projectile
    {
        private ExplodingProjectileDataSO _explodingData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is ExplodingProjectileDataSO explodingData)
            {
                _explodingData = explodingData;
            }
            else
            {
                Debug.LogError("[ExplodingBullet] ExplodingProjectileDataSO is not set for ExplodingBullet");
            }
        }

        protected override void HandleActivation()
        {
            Timing.RunCoroutine(ExplodeCoroutine());
        }

        private IEnumerator<float> ExplodeCoroutine()
        {
            yield return Timing.WaitForSeconds(5f);
            Explode();
            ReturnToPool();
        }

        private void Explode()
        {
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, _explodingData.ExplosionRadius);
            foreach (Collider2D hit in affectedObjects)
            {
                if (_explodingData.EffectsOtherProjectiles && hit.TryGetComponent(out Projectile projectile) && projectile != this)
                {
                    Vector2 directionAwayFromExplosion = (projectile.transform.position - transform.position).normalized;
                    projectile.ChangeDirection(directionAwayFromExplosion * _explodingData.ExplosionForce);
                }
                
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    Vector2 direction = (hit.transform.position - transform.position).normalized;
                    damageable.HandleDamageWithForce(_explodingData.Damage, direction, _explodingData.ExplosionForce);
                }
            }
        }
    }
}