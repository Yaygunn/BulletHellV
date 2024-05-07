using System;
using BH.Runtime.Factories;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Systems
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _damage;

        [SerializeField]
        private bool _destroyOnHit;

        private ProjectilePool _pool;
        
        private Vector2 _direction;
        
        [Inject]
        private void Construct(ProjectilePool pool)
        {
            _pool = pool;
        }
        
        public void SetUp()
        {
            transform.position = Vector3.zero;
            RandomizeDirection();
        }

        private void RandomizeDirection()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            _direction = new Vector2(randomX, randomY).normalized;
        }

        private void Update()
        {
            transform.position += new Vector3(_direction.x, _direction.y, 0f) * (_speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Vector2 inNormal = other.GetContact(0).normal;
            _direction = Vector2.Reflect(_direction, inNormal).normalized;

            if (other.gameObject.TryGetComponent(out IDamageable component))
            {
                component.Damage(_damage);
                if (_destroyOnHit)
                {
                    ReturnToPool();
                }
            }

            //ReturnToPool();
        }

        private void OnDisable()
        {
            // TODO: Additional possible cleanup.
        }

        private void ReturnToPool()
        {
            _pool.Despawn(this);
        }
    }
}