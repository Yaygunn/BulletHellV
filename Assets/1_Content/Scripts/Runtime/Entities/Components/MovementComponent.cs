using System;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MovementComponent : MonoBehaviour, IEntityComponent
    {
        private Rigidbody2D _rigidbody;
        
        private Vector2 _direction;
        private float _defaultSpeed = 5f;
        private Vector2? _destination;
        private Action _onReachDestination;
        private float _closeEnough = 0.1f;

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
            if (_destination.HasValue)
            {
                Vector2 toDestination = _destination.Value - (Vector2)transform.position;
                if (toDestination.magnitude <= _closeEnough)
                {
                    _rigidbody.velocity = Vector2.zero;
                    _destination = null;
                    _onReachDestination?.Invoke();
                }
                else
                {
                    _rigidbody.velocity = toDestination.normalized * _defaultSpeed;
                }
            }
            else
            {
                _rigidbody.velocity = _direction.normalized * _defaultSpeed;
            }
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
            _destination = null;
        }
        
        public void Move(Vector2 direction, float speed)
        {
            _direction = direction;
            _defaultSpeed = speed;
            _destination = null;
        }
        
        public void MoveTo(Vector2 destination, float speed, Action onReachDestination = null)
        {
            _destination = destination;
            _defaultSpeed = speed;
            _onReachDestination = onReachDestination;
        }

        public void AddForce(Vector2 direction, float force)
        {
            _rigidbody.AddForce(direction.normalized * force);
        }
        
        public void Stop()
        {
            _destination = null;
            _direction = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
