using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Factories;
using BH.Runtime.Input;
using BH.Runtime.Managers;
using BH.Runtime.Systems;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Scripts.Runtime.UI;
using BH.Utilities.ImprovedTimers;
using Mono.CSharp.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Entities
{
    public class WeaponComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField]
        private float _fireRate = 0.2f;
        [SerializeField]
        private ProjectileType _startingBullets = ProjectileType.PlayerBasicBullet;
        [SerializeField]
        private int _startingBulletLevel = 0;

        [SerializeField]
        private BulletContainer[] _bulletContainers = new BulletContainer[6];
        private int _currentBulletIndex = 0;
        
        [SerializeField]
        private GeneralWeaponMod _generalWeaponMod;
        
        private IProjectileFactory _projectileFactory;

        public bool IsOnCooldown { get; private set; }

        private CountdownTimer _cooldownCountdown;
        private DatabaseSO _database;
        private Dictionary<ProjectileType, int> _bulletLevels;

        [Inject] 
        private void Construct(IProjectileFactory projectileFactory, DatabaseSO database)
        {
            _projectileFactory = projectileFactory;
            _database = database;
        }
        
        // TODO: TESTING
        [Inject]
        private ILevelStateHandler _levelStateHandler;
        [Button]
        private void TestUpgrades()
        {
            _levelStateHandler.SetLevelState(LevelState.Upgrading);
        }
        
        private void Start()
        {
            _cooldownCountdown = new CountdownTimer(GetFireRate());
            _cooldownCountdown.OnTimerStop += OnCooldownEnd;

            ProjectileDataSO basicBullet = _database.GetProjectileData(_startingBullets, 0);
            foreach (BulletContainer container in _bulletContainers)
            {
                container.SetBulletData(basicBullet);
            }

            _bulletLevels = new Dictionary<ProjectileType, int>()
            {
                { ProjectileType.AttractorBullet, 1 },
                { ProjectileType.PlayerBasicBullet, 0 },
                { ProjectileType.EnemyBasicBullet, 0 },
                { ProjectileType.ChainReactionBullet, 1 },
                { ProjectileType.ExplodingBullet, 1 },
                { ProjectileType.HealingBullet, 1 },
                { ProjectileType.HomingBullet, 1 }
            };
        }

        private void OnDestroy()
        {
            _cooldownCountdown.OnTimerStop -= OnCooldownEnd;
        }

        public void Fire(Vector2 direction)
        {
            SpawnBullet(direction, transform.position);
            IsOnCooldown = true;
            _cooldownCountdown.Reset(GetFireRate());
            _cooldownCountdown.Start();
        }
        
        public void AddBullet(ProjectileType type)
        {
            BulletContainer container = _bulletContainers.FirstOrDefault(x => x.BulletData.GetProjectileType() == ProjectileType.PlayerBasicBullet);
            if (container == null)
            {
                Debug.LogError("No more basic bullets to replace.");
                return;
            }
            
            ProjectileDataSO data = _database.GetProjectileData(type, 0);
            
            container.SetBulletData(data);
            _bulletContainers[_currentBulletIndex] = container;
        }

        private void SpawnBullet(Vector3 velocity, Vector2 position)
        {
            ProjectileType type = GetNextBulletType();
            Projectile projectile = _projectileFactory.CreateProjectile(type);
            projectile.SetUp(position, velocity.normalized);
        }
        
        private void OnCooldownEnd()
        {
            IsOnCooldown = false;
        }
        
        private ProjectileType GetNextBulletType()
        {
            ProjectileType type = _bulletContainers[_currentBulletIndex].BulletData.GetProjectileType();
            _currentBulletIndex = (_currentBulletIndex + 1) % _bulletContainers.Length;
            return type;
        }
        
        private float GetFireRate()
        {
            float fireRate = (_fireRate + _generalWeaponMod.IncreasedFireRate) * _generalWeaponMod.FireRateMultiplier;
            return fireRate < 0f ? 0f : fireRate;
        }
    }
}