using System.Collections.Generic;
using BH.Runtime.Entities;
using BH.Runtime.Factories;
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
        EnemyBasicBullet,
        ExpandingBullet,
        ExplodingBullet,
        HealingBullet,
        HomingBullet,
        PlayerBasicBullet
    }
    
    public abstract class Projectile : MonoBehaviour
    {

        [BoxGroup("General"), SerializeField]
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
        protected float _initialSize;
        protected float _currentSize;
        protected float _currentSpeed;
        private CoroutineHandle _speedCheckCoroutine;
        private int _bounces;
        private bool _hasEvolved;
        private bool _hasActivated;
        private IProjectileFactory _projectileFactory;
        
        private ProjectileDataSO _currentProjData;
        private ProjectileDataSO _evolutionProjData;
        private GeneralWeaponMod _weaponMod;
        
        public Vector2 CurrentDirection { get; private set; }
        
        [Inject]
        private void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        #region Unity Callbacks

        protected virtual void OnEnable()
        {
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
            if (_isInPool) return;
            
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
        }
        
        public void SetUp(Vector2 initialPosition, Vector2 initialDirection, ProjectileDataSO projectileData,
            ProjectileDataSO evolutionData = null, GeneralWeaponMod weaponMod = null, bool isEvolved = false)
        {
            _evolutionProjData = evolutionData;
            _currentProjData = projectileData;
            _weaponMod = weaponMod ?? new GeneralWeaponMod();

            _currentSpeed = (_currentProjData.Speed + _weaponMod.IncreasedProjSpeed) * _weaponMod.ProjSpeedMultiplier;
            transform.position = initialPosition;
            CurrentDirection = initialDirection.normalized;
            
            SetUpInternal(_currentProjData);
            
            if (isEvolved)
            {
                HandleEvolution();
            }
        }

        protected abstract void SetUpInternal(ProjectileDataSO projectileData);
        
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
                if (!(_currentSpeed < _currentProjData.Speed * _lowSpeedThresholdFactor)) continue;

                float checkEndTime = Time.time + _recoveryCheckDuration;
                bool speedRecovered = false;

                while (Time.time < checkEndTime) 
                {
                    yield return Timing.WaitForSeconds(0.1f);
                    if (!(_currentSpeed >= _currentProjData.Speed * _lowSpeedThresholdFactor)) continue;
                    
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

            if (!_hasEvolved && _bounces >= _currentProjData.EvolutionBounces)
            {
                if (_evolutionProjData != null)
                {
                    Projectile projectile = _projectileFactory.CreateProjectile(_evolutionProjData.GetProjectileType());
                    projectile.SetUp(transform.position, CurrentDirection, _evolutionProjData,
                        null, _weaponMod, true);
                    ReturnToPool();
                }
                else
                {
                    HandleEvolution();
                    _hasEvolved = true;
                }
            }
            else if (!_hasActivated && _bounces >= _currentProjData.ActivationBounces)
            {
                HandleActivation();
            }
        }
        
        private void HandleDamage(IDamageable damageable)
        {
            int damage = (int)((_currentProjData.Damage + _weaponMod.IncreasedDamage) * _weaponMod.DamageMultiplier);
            damageable?.HandleDamage(damage);
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
            _currentProjData = null;
            _evolutionProjData = null;
            _weaponMod = null;
            _bounces = 0;
            _hasEvolved = false;
            _hasActivated = false;
            _isInPool = true;
        }
    }
}