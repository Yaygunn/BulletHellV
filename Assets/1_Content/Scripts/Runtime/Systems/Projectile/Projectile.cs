using System;
using BH.Runtime.Factories;
using BH.Runtime.Test;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Systems
{
    public class Projectile : MonoBehaviour
    {
        //[SerializeField]
        //private float _speed;

        [SerializeField]
        private int _damage;

        [SerializeField]
        private bool _destroyOnHit;
        
        [SerializeField]
        private LayerMask _collisionMask;

        private ProjectilePool _pool;
        private Vector2 _direction;
        
        // TODO: REMOVE THIS TEST
        private TestSpawner _spawner;
        
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
        
        public void SetUp(Vector2 initialDirection)
        {
            transform.position = Vector3.zero;
            _direction = initialDirection;
        }
        
        // TODO: THIS IS FOR TESTING, NEED TO REMOVE...
        public void SetUp(Vector2 initialDirection, TestSpawner spawner)
        {
            transform.position = Vector3.zero;
            _direction = initialDirection;
            _spawner = spawner;
        }

        private void RandomizeDirection()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            _direction = new Vector2(randomX, randomY).normalized;
        }

        private void Update()
        {
            transform.position += new Vector3(_direction.x, _direction.y, 0f) * (/*_speed **/ Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable component))
            {
                component.Damage(_damage);
                if (_destroyOnHit)
                {
                    _spawner.BulletCounter--;
                    ReturnToPool();
                    return;
                }
            }
            
            if ((_collisionMask & (1 << other.gameObject.layer)) != 0)
            {
                _spawner.BulletCounter--;
                ReturnToPool();
            }

            
            //Vector2 inNormal = other.GetContact(0).normal;
            //_direction = Vector2.Reflect(_direction, inNormal).normalized;
            
            
            
            // TODO: Can return to pool here if needed..
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