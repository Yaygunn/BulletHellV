using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Systems
{
    public class HomingBullet : Projectile
    {
        [BoxGroup("Homing Bullet"), SerializeField]
        private float _homingStrength = 0.1f;
        [BoxGroup("Homing Bullet"), SerializeField]
        private float _targetDetectionRadius = 10f;
        [BoxGroup("Homing Bullet"), SerializeField]
        private float _targetCheckInterval = 1f;

        private Transform _target;
        private CoroutineHandle _targetCheckCoroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            _targetCheckCoroutine = Timing.RunCoroutine(TargetCheckCoroutine().CancelWith(gameObject));
        }

        protected override void Update()
        {
            if (_target != null)
            {
                Vector2 directionToTarget = (_target.position - transform.position).normalized;
                ChangeDirection(Vector2.Lerp(CurrentDirection, directionToTarget, _homingStrength * Time.deltaTime));
                transform.position += new Vector3(CurrentDirection.x, CurrentDirection.y, 0) * (_baseSpeed * Time.deltaTime);
            }
            else
            {
                base.Update();
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
                yield return Timing.WaitForSeconds(_targetCheckInterval);
            }
        }

        private void FindClosestTarget()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _targetDetectionRadius);
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

        protected override void ResetProperties()
        {
            base.ResetProperties();
            _target = null;
        }
    }
}