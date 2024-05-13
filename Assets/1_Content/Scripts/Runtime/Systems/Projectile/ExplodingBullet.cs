using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class ExplodingBullet : Projectile
    {
        [BoxGroup("Explosion"), SerializeField]
        private float _explosionRadius = 5f;
        [BoxGroup("Explosion"), SerializeField]
        private float _explosionForce = 10f;
        [BoxGroup("Explosion"), SerializeField]
        private bool _effectsOtherProjectiles = false;
        
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
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
            foreach (Collider2D hit in affectedObjects)
            {
                if (_effectsOtherProjectiles && hit.TryGetComponent(out Projectile projectile) && projectile != this)
                {
                    Vector2 directionAwayFromExplosion = (projectile.transform.position - transform.position).normalized;
                    projectile.ChangeDirection(directionAwayFromExplosion * _explosionForce);
                }
            }
        }
    }
}