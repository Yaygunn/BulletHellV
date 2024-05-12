using System;
using BH.Runtime.Factories;
using BH.Runtime.Test;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Systems
{
    // NOTE: Adding a new type here will try to spawn a pool for it, make sure the new type exists in resources...
    public enum ProjectileType
    {
        AttractorBullet,
        ChainReactionBullet,
        EnemyBasicBullet,
        ExplodingBullet,
        HealingBullet,
        HomingBullet,
        PlayerBasicBullet
    }
    
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
        
        private IProjectileFactory _projectileFactory;
        
        [Inject]
        private void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }
        
        public void SetPool(ProjectilePool pool)
        {
            _pool = pool;
        }
        
        public void SetUp()
        {
            transform.position = Vector3.zero;
            RandomizeDirection();
        }
        
        public void SetUp(Vector2 initialPosition, Vector2 initialDirection)
        {
            transform.position = initialPosition;
            _direction = initialDirection;
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
            if ((_collisionMask & (1 << other.gameObject.layer)) != 0)
            {
                Vector2 inNormal = other.GetContact(0).normal;
                _direction = Vector2.Reflect(_direction, inNormal).normalized;
                
                Projectile projectile = _projectileFactory.CreateProjectile(ProjectileType.ExplodingBullet);
                projectile.SetUp(transform.position, _direction);
                
                ReturnToPool();
            }
            
            
            if (other.gameObject.TryGetComponent(out IDamageable component))
            {
                component.Damage(_damage);
                if (_destroyOnHit)
                {
                    ReturnToPool();
                    return;
                }
            }
            
            

            
            
            
            
            
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