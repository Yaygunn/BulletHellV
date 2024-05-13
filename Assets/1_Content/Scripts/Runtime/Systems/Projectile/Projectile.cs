using System.Collections.Generic;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Scriptables;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

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
        [BoxGroup("General Settings"), SerializeField]
        private int _baseDamage = 10;
        [BoxGroup("General Settings"), SerializeField]
        protected float _baseSpeed = 10f;
        [BoxGroup("General Settings"), SerializeField]
        private int _baseEvolutionBounces = 1;
        [BoxGroup("General Settings"), SerializeField]
        private int _baseActivationBounces = 3;
        [BoxGroup("General Settings"), SerializeField]
        private LayerMask _obsticleMask;
        
        [BoxGroup("Speed Monitor"), SerializeField]
        private bool _enableSpeedMonitoring = true;
        [BoxGroup("Speed Monitor"), SerializeField, ShowIf(nameof(_enableSpeedMonitoring))]
        private float _speedCheckInterval = 0.5f;
        [BoxGroup("Speed Monitor"), SerializeField, ShowIf(nameof(_enableSpeedMonitoring))]
        private float _lowSpeedThresholdFactor = 0.5f;
        [BoxGroup("Speed Monitor"), SerializeField, ShowIf(nameof(_enableSpeedMonitoring))]
        private float _recoveryCheckDuration = 3f;

        private ProjectilePool _pool;
        private bool _isInPool;
        private EvolutionDataSO _evolutionData;
        protected float _initialSize;
        protected float _currentSize;
        private float _currentSpeed;
        private CoroutineHandle _speedCheckCoroutine;
        private bool _isEvolved;
        private int _bounces;
        private IProjectileFactory _projectileFactory;
        
        public Vector2 CurrentDirection { get; private set; }
        
        [Inject]
        private void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        #region Unity Callbacks

        protected virtual void OnEnable()
        {
            _currentSpeed = _baseSpeed;
            if (_enableSpeedMonitoring) 
            {
                _speedCheckCoroutine = Timing.RunCoroutine(MonitorSpeedCoroutine().CancelWith(gameObject));
            }
        }
        
        protected virtual void Start()
        {
            _initialSize = transform.localScale.x;
            _currentSize = _initialSize;
        }

        protected virtual void Update()
        {
            transform.position += new Vector3(CurrentDirection.x, CurrentDirection.y, 0f) * (_currentSpeed * Time.deltaTime);
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

        protected virtual void OnDisable()
        {
            if (_enableSpeedMonitoring && Timing.IsRunning(_speedCheckCoroutine)) 
            {
                Timing.KillCoroutines(_speedCheckCoroutine);
            }
        }

        #endregion

        public void SetPool(ProjectilePool pool)
        {
            _pool = pool;
            _isInPool = false;
            _currentSpeed = _baseSpeed;
        }
        
        public void SetUp(Vector2 initialPosition, Vector2 initialDirection, EvolutionDataSO evolutionData = null,
            bool hasEvolved = false)
        {
            transform.position = initialPosition;
            CurrentDirection = initialDirection.normalized;
            _evolutionData = evolutionData;
            _isEvolved = hasEvolved;
            
            if (_isEvolved && _evolutionData != null)
            {
                HandleEvolution();
            }
        }
        
        public void ChangeDirection(Vector2 newDirection)
        {
            CurrentDirection = newDirection.normalized;
        }
        
        public void ChangeDirectionAndSpeed(Vector2 newDirection, float newSpeed)
        {
            CurrentDirection = newDirection.normalized;
            _currentSpeed = newSpeed;
        }
        
        private IEnumerator<float> MonitorSpeedCoroutine() 
        {
            while (true) 
            {
                yield return Timing.WaitForSeconds(_speedCheckInterval);
                if (!(_currentSpeed < _baseSpeed * _lowSpeedThresholdFactor)) continue;

                float checkEndTime = Time.time + _recoveryCheckDuration;
                bool speedRecovered = false;

                while (Time.time < checkEndTime) 
                {
                    yield return Timing.WaitForSeconds(0.1f);
                    if (!(_currentSpeed >= _baseSpeed * _lowSpeedThresholdFactor)) continue;
                    
                    speedRecovered = true;
                    break;
                }

                if (speedRecovered) continue;
                
                HandleActivation();
                break;
            }
        }
        
        protected virtual void HandleObstacleCollision(Collision2D other)
        {
            Vector2 inNormal = other.GetContact(0).normal;
            CurrentDirection = Vector2.Reflect(CurrentDirection, inNormal).normalized;
            _bounces++;

            if (!_isEvolved && _bounces >= GetEvolutionBounces() && _evolutionData != null)
            {
                Projectile projectile = _projectileFactory.CreateProjectile(_evolutionData.GetProjectileType());
                projectile.SetUp(transform.position, CurrentDirection, _evolutionData, true);
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
        
        protected virtual void HandleEvolution()
        {
            // no-op
        }

        protected abstract void HandleActivation();

        protected void ReturnToPool()
        {
            if (_isInPool)
                return;
            
            ResetProperties();
            _pool.Despawn(this);
        }
        
        protected virtual void ResetProperties()
        {
            _evolutionData = null;
            _isEvolved = false;
            _bounces = 0;
            _isInPool = true;
            _currentSpeed = _baseSpeed;
        }
    }
}