using System.Collections.Generic;
using BH.Scriptables;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class AttractorBullet : Projectile
    {
        private AttractorProjectileDataSO _attractorData;
        
        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is AttractorProjectileDataSO attractorData)
            {
                _attractorData = attractorData;
            }
            else
            {
                Debug.LogError("[AttractorBullet] AttractorEvolutionDataSO is not set for AttractorBullet");
            }
        }

        protected override void HandleActivation()
        {
            Timing.RunCoroutine(AttractionCoroutine());
        }

        private IEnumerator<float> AttractionCoroutine()
        {
            float endTime = Time.time + _attractorData.AttractionDuration;
            
            while (Time.time < endTime)
            {
                AttractNearbyProjectiles();
                yield return Timing.WaitForSeconds(0.1f);
            }
            
            ReturnToPool();
        }

        private void AttractNearbyProjectiles()
        {
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, _attractorData.AttractionRadius);
            foreach (Collider2D hit in affectedObjects)
            {
                if (hit.TryGetComponent(out Projectile projectile) && projectile != this && projectile is not AttractorBullet)
                {
                    Vector2 directionToAttractor = (transform.position - projectile.transform.position).normalized;
                    float distance = Vector2.Distance(transform.position, projectile.transform.position);

                    if (distance > _attractorData.MinDistanceThreshold)
                    {
                        projectile.ChangeDirection(projectile.CurrentDirection + directionToAttractor * _attractorData.AttractionForce);
                    }
                }
            }
        }

    }
}