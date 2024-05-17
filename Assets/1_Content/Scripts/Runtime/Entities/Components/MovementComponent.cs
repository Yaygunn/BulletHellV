using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MovementComponent : MonoBehaviour, IEntityComponent
    {
        private Rigidbody2D _rigidbody;
        
        private Vector2 _direction;
        private float _speed = 5f;
        //private bool _isControlLocked;

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
            
            _rigidbody.velocity = _direction.normalized * _speed;
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
            _speed = speed;
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