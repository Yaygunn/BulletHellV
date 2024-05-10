using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class MovementComponent : MonoBehaviour, IEntityComponent
    {
        [BoxGroup("Settings"), SerializeField]
        private float _speed = 5f;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;

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
            _rigidbody.velocity = _direction * _speed;
        }
        
        public void Move(Vector2 direction)
        {
            _direction = direction;
        }
        
        public void Stop()
        {
            _direction = Vector2.zero;
        }
    }
}