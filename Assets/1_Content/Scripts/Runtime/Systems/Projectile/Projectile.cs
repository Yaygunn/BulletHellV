using System;
using BH.Runtime.Factories;
using BH.Runtime.Test;
using BH.Scriptables;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BH.Runtime.Systems
{
    // NOTE: Adding a new type here will try to spawn a pool for it, make sure the new type exists in prefab resources...
    public enum ProjectileType
    {
        AttractorBullet,
        ChainReactionBullet,
        EnemyBasicBullet,
        ExpandingBullet,
        ExplodingBullet,
        HealingBullet,
        HomingBullet,
        PlayerBasicBullet
    }
    
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField]
        private int _baseDamage = 10;
        [SerializeField]
        private float _baseSpeed = 10f;
        [SerializeField]
        private int _baseEvolutionBounces = 1;
        [SerializeField]
        private int _baseActivationBounces = 3;
        
        [SerializeField]
        private LayerMask _obsticleMask;

        private ProjectilePool _pool;
        private Vector2 _direction;
        private EvolutionDataSO _evolutionData;

        //private bool _isEvolved;
        private int _bounces;
        
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
        
        public void SetUp(Vector2 initialPosition, EvolutionDataSO evolutionData)
        {
            _evolutionData = evolutionData;
            transform.position = initialPosition;
            RandomizeDirection();
        }
        
        public void SetUp(Vector2 initialPosition, Vector2 initialDirection, EvolutionDataSO evolutionData = null)
        {
            transform.position = initialPosition;
            _direction = initialDirection;
            _evolutionData = evolutionData;
        }

        private void RandomizeDirection()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(-1f, 1f);
            _direction = new Vector2(randomX, randomY).normalized;
        }

        private void Update()
        {
            transform.position += new Vector3(_direction.x, _direction.y, 0f) * Time.deltaTime;
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if ((_obsticleMask & (1 << other.gameObject.layer)) != 0)
            {
                HandleObstacleCollision(other);
                return;
            }

            if (!other.gameObject.TryGetComponent(out IDamageable damageable)) 
                return;
            
            HandleDamage(damageable);
            ReturnToPool();
        }
        
        private void HandleObstacleCollision(Collision2D other)
        {
            Vector2 inNormal = other.GetContact(0).normal;
            _direction = Vector2.Reflect(_direction, inNormal).normalized;
            _bounces++;

            if (_bounces >= GetEvolutionBounces() && _evolutionData != null)
            {
                Projectile projectile = _projectileFactory.CreateProjectile(_evolutionData.GetProjectileType());
                projectile.SetUp(transform.position, _direction, _evolutionData);
                ReturnToPool();
            }
            else if (_bounces >= GetActivationBounces())
            {
                HandleActivation();
            }
        }
        
        private void HandleDamage(IDamageable damageable)
        {
            int damage = _baseDamage;
            damageable.Damage(damage);
        }
        
        private int GetEvolutionBounces()
        {
            return _baseEvolutionBounces;
        }
        
        private int GetActivationBounces()
        {
            return _baseActivationBounces;
        }
        
        private void HandleActivation()
        {
            Debug.Log("Projectile Activated!");
            ReturnToPool();
        }

        protected void ReturnToPool()
        {
            _evolutionData = null;
            _bounces = 0;
            
            _pool.Despawn(this);
        }
    }
}