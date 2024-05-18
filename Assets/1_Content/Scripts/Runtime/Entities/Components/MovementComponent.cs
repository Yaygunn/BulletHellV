using System;
using MEC;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MovementComponent : MonoBehaviour, IEntityComponent
    {
        private Rigidbody2D _rigidbody;
        
        private Vector2 _direction;
        private float _defaultSpeed = 5f;
        private Vector2? _destination;
        private float _destinationThreshold = 0.1f;
        //private bool _isControlLocked;
        
        private CoroutineHandle _moveToDestinationCoroutine;

        public event Action OnDestinationReached;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("[MovementComponent] Rigidbody2D is missing. Disabling component...");
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            //if (_isControlLocked) return;
            
            if (_destination.HasValue)
            {
                Vector2 directionToTarget = (_destination.Value - (Vector2)transform.position).normalized;
                _rigidbody.velocity = directionToTarget * _defaultSpeed;

                if (Vector2.Distance(transform.position, _destination.Value) <= _destinationThreshold)
                {
                    Stop();
                    OnDestinationReached?.Invoke();
                }
            }
            else
            {
                _rigidbody.velocity = _direction.normalized * _defaultSpeed;
            }
        }
        
        public void Move(Vector2 direction)
        {
            //if (_isControlLocked) return;
            
            _direction = direction;
        }
        
        public void Move(Vector2 direction, float speed)
        {
            //if (_isControlLocked) return;
            
            _direction = direction;
            _defaultSpeed = speed;
        }
        
        public void MoveTo(Vector2 destination, float speed, float threshold = 0.1f)
        {
            
        }
        
        public void AddForce(Vector2 direction, float force)
        {
            _rigidbody.AddForce(direction.normalized * force);
            // TODO: fix hard code of lock
            //Timing.RunCoroutine(TempLockControlCoroutine(0.5f));
        }
        
        public void Stop()
        {
            //if (_isControlLocked) return;
            
            _direction = Vector2.zero;
        }
        
        // private IEnumerator<float> TempLockControlCoroutine(float duration)
        // {
        //     _isControlLocked = true;
        //     yield return Timing.WaitForSeconds(duration);
        //     _isControlLocked = false;
        // }
    }
}