using System.Collections.Generic;
using BH.Scriptables;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class HomingBullet : Projectile
    {
        private Transform _target;
        private CoroutineHandle _targetCheckCoroutine;
        
        private HomingProjectileDataSO _homingData;
        private Quaternion _originalRotation;

        protected override void SetUpInternal(ProjectileDataSO projectileData)
        {
            if (projectileData is HomingProjectileDataSO homingEvolutionData)
            {
                _homingData = homingEvolutionData;
            }
            else
            {
                Debug.LogError("[HomingBullet] HomingEvolutionDataSO is not set for HomingBullet");
            }
            
            _targetCheckCoroutine = Timing.RunCoroutine(TargetCheckCoroutine().CancelWith(gameObject));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _originalRotation = transform.rotation;
        }

        protected override void Update()
        {
            if (_target != null)
            {
                Vector2 directionToTarget = (_target.position - transform.position).normalized;
                ChangeDirection(Vector2.Lerp(CurrentDirection, directionToTarget, _homingData.HomingStrength * Time.deltaTime));
                transform.position += new Vector3(CurrentDirection.x, CurrentDirection.y, 0) * (_homingData.Speed * Time.deltaTime);
                RotateTowardsDirection(CurrentDirection);
            }
            else
            {
                base.Update();
                RotateTowardsDirection(CurrentDirection);
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (Timing.IsRunning(_targetCheckCoroutine))
            {
                Timing.KillCoroutines(_targetCheckCoroutine);
            }
        }

        protected override void HandleActivation()
        {
            ReturnToPool();
        }

        private IEnumerator<float> TargetCheckCoroutine()
        {
            while (true)
            {
                FindClosestTarget();
                yield return Timing.WaitForSeconds(_homingData.TargetCheckInterval);
            }
        }

        private void FindClosestTarget()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _homingData.TargetDetectionRadius);
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject || hit.GetComponent<IDamageable>() == null) continue;
                
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (!(distance < closestDistance)) continue;
                    
                closestDistance = distance;
                closestTarget = hit.transform;
            }

            _target = closestTarget;
        }

        private void RotateTowardsDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected override void ResetProperties()
        {
            base.ResetProperties();
            _target = null;
            transform.rotation = _originalRotation;
        }
    }
}
