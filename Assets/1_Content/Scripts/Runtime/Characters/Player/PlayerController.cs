using BH.Runtime.Test;
using UnityEngine;

namespace BH.Runtime.Characters
{
    /// <summary>
    /// This PlayerController class will be more properly set up.. just using for testing...
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings"), SerializeField]
        private float _speed;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            _direction = new Vector2(horizontal, vertical).normalized;
            
            //transform.position += new Vector3(_direction.x, _direction.y, 0f) * (_speed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;
        }
    }
}