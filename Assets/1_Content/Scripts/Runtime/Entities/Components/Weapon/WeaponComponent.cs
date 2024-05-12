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

        [SerializeField, ReadOnly]
        private EvolutionDataSO[] _evolutionsList = new EvolutionDataSO[6];
        
        [SerializeField]
        private GeneralWeaponMod _generalWeaponMod;
        
        [BoxGroup("Debugging"), SerializeField, ReadOnly]
        private int _currentBulletIndex = 0;
        
        public bool IsOnCooldown { get; private set; }

        private IProjectileFactory _projectileFactory;
        private CountdownTimer _cooldownCountdown;
        private DatabaseSO _database;
        private Dictionary<ProjectileType, int> _bulletLevels;

        [Inject] 
        private void Construct(IProjectileFactory projectileFactory, DatabaseSO database)
        {
            _projectileFactory = projectileFactory;
            _database = database;
        }
        
        private void Start()
        {
            _cooldownCountdown = new CountdownTimer(GetFireRate());
            _cooldownCountdown.OnTimerStop += OnCooldownEnd;

            _bulletLevels = new Dictionary<ProjectileType, int>()
            {
                { ProjectileType.AttractorBullet, 1 },
                { ProjectileType.ChainReactionBullet, 1 },
                { ProjectileType.ExpandingBullet, 1 },
                { ProjectileType.ExplodingBullet, 1 },
                { ProjectileType.HealingBullet, 1 },
                { ProjectileType.HomingBullet, 1 }
            };
        }

        private void OnDestroy()
        {
            _cooldownCountdown.OnTimerStop -= OnCooldownEnd;
        }

        public void Fire(Vector2 direction, ProjectileType initialType)
        {
            if (initialType != ProjectileType.PlayerBasicBullet && initialType != ProjectileType.EnemyBasicBullet)
            {
                Debug.LogError("Invalid initial bullet type. Make sure it's player/enemy basic bullet.");
                return;
            }
            
            SpawnBullet(direction, transform.position, initialType);
            IsOnCooldown = true;
            _cooldownCountdown.Reset(GetFireRate());
            _cooldownCountdown.Start();
        }
        
        public bool CanAddBulletEvolution() => _evolutionsList.Any(x => x == null);
        
        public void AddBulletEvolution(ProjectileType type)
        {
            if (!CanAddBulletEvolution())
            {
                Debug.LogError("Attempted to add bullet evolution but no slots available.");
                return;
            }
    
            int index = Array.FindIndex(_evolutionsList, x => x == null);
            if (index == -1)
            {
                Debug.LogError("No null slots found despite earlier check.");
                return;
            }

            if (_database.TryGetNextEvolutionData(type, _bulletLevels[type], out EvolutionDataSO newEvolutionData))
            {
                _evolutionsList[index] = newEvolutionData;
            }
            else
            {
                Debug.LogError($"[WeaponComponent] Failed to get evolution data for {type} at level {_bulletLevels[type]}.");
            }
        }
        
        public bool CanUpgradeEvolution(ProjectileType type)
        {
            return _database.TryGetNextEvolutionData(type, _bulletLevels[type] + 1, out EvolutionDataSO evolutionData);
        }
        
        public void UpgradeEvolutions(ProjectileType type)
        {
            if (_database.TryGetNextEvolutionData(type, _bulletLevels[type] + 1, out EvolutionDataSO evolutionData))
            {
                _bulletLevels[type] += 1;

                for (int i = 0; i < _evolutionsList.Length; i++)
                {
                    if (_evolutionsList[i] != null && _evolutionsList[i].GetProjectileType() == type)
                    {
                        _evolutionsList[i] = evolutionData;
                    }
                }
            }
            else
            {
                Debug.LogError("Max level reached for projectile type or evolution data not found.");
            }
        }

        private void SpawnBullet(Vector3 velocity, Vector2 position, ProjectileType type)
        {
            EvolutionDataSO evolution = GetNextEvolutionIfAny();
            Projectile projectile = _projectileFactory.CreateProjectile(type);
            projectile.SetUp(position, velocity.normalized, evolution);
        }
        
        private void OnCooldownEnd() => IsOnCooldown = false;

        private EvolutionDataSO GetNextEvolutionIfAny()
        {
            EvolutionDataSO evolution = _evolutionsList[_currentBulletIndex];
            _currentBulletIndex = (_currentBulletIndex + 1) % _evolutionsList.Length;
            return evolution;
        }
        
        private float GetFireRate()
        {
            float fireRate = (_fireRate + _generalWeaponMod.IncreasedFireRate) * _generalWeaponMod.FireRateMultiplier;
            return fireRate < 0f ? 0f : fireRate;
        }
    }
}