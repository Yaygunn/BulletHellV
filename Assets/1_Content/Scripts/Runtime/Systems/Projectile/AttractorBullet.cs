using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class AttractorBullet : Projectile
    {
        [BoxGroup("Attractor Bullet"), SerializeField]
        private float _attractionRadius = 5f;
        [BoxGroup("Attractor Bullet"), SerializeField]
        private float _minDistanceThreshold = 1f;
        [BoxGroup("Attractor Bullet"), SerializeField]
        private float _attractionForce = 0.2f;
        [BoxGroup("Attractor Bullet"), SerializeField]
        private float _attractionDuration = 5f;

        protected override void HandleActivation()
        {
            Timing.RunCoroutine(AttractionCoroutine());
        }

        private IEnumerator<float> AttractionCoroutine()
        {
            float endTime = Time.time + _attractionDuration;
            
            while (Time.time < endTime)
            {
                AttractNearbyProjectiles();
                yield return Timing.WaitForSeconds(0.1f);
            }
            
            ReturnToPool();
        }

        private void AttractNearbyProjectiles()
        {
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, _attractionRadius);
            foreach (Collider2D hit in affectedObjects)
            {
                if (hit.TryGetComponent(out Projectile projectile) && projectile != this && projectile is not AttractorBullet)
                {
                    Vector2 directionToAttractor = (transform.position - projectile.transform.position).normalized;
                    float distance = Vector2.Distance(transform.position, projectile.transform.position);

                    if (distance > _minDistanceThreshold)
                    {
                        projectile.ChangeDirection(projectile.CurrentDirection + directionToAttractor * _attractionForce);
                    }
                }
            }
        }

    }
}