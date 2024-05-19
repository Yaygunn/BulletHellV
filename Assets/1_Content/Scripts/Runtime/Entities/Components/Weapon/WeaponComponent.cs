using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Audio;
using BH.Runtime.Factories;
using BH.Runtime.Systems;
using BH.Runtime.UI;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Utilities.ImprovedTimers;
using GH.Scriptables;
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
        private ProjectileType _baseProjectile = ProjectileType.PlayerBasicBullet;

        [SerializeField, ReadOnly]
        private ProjectileDataSO[] _evolutionsList = new ProjectileDataSO[5];
        
        [FoldoutGroup("Weapon Mod"), SerializeField, HideLabel]
        private GeneralWeaponMod _generalWeaponMod;
        
        [BoxGroup("Debugging"), SerializeField, ReadOnly]
        private int _currentBulletIndex = 0;
        
        public bool IsOnCooldown { get; private set; }

        private IProjectileFactory _projectileFactory;
        private IWwiseEventHandler _wwiseEventHandler;
        private ProjectileDataSO _baseProjectileData;
        private CountdownTimer _cooldownCountdown;
        private DatabaseSO _database;
        private Dictionary<ProjectileType, int> _bulletLevels;
        private SignalBus _signalBus;

        [Inject] 
        private void Construct(IProjectileFactory projectileFactory, IWwiseEventHandler wwiseEventHandler, DatabaseSO database, SignalBus signalBus)
        {
            _projectileFactory = projectileFactory;
            _wwiseEventHandler = wwiseEventHandler;
            _database = database;
            _signalBus = signalBus;
        }
        
        private void Start()
        {
            _cooldownCountdown = new CountdownTimer(GetFireRate());
            _cooldownCountdown.OnTimerStop += OnCooldownEnd;

            _bulletLevels = new Dictionary<ProjectileType, int>();
            foreach (ProjectileType type in Enum.GetValues(typeof(ProjectileType)))
            {
                _bulletLevels[type] = 1;
            }

            for (int i = 0; i < _evolutionsList.Length; i++)
            {
                if (_database.TryGetProjectileData(_baseProjectile, _bulletLevels[_baseProjectile], out ProjectileDataSO projectileData))
                {
                    _evolutionsList[i] = projectileData;
                }
                else
                {
                    Debug.LogError($"[WeaponComponent] Failed to get evolution data " +
                                   $"for {_baseProjectile} at level {_bulletLevels[_baseProjectile]}.");
                }
            }
            
            _baseProjectileData = _evolutionsList[0];
        }

        private void OnDestroy()
        {
            _cooldownCountdown.OnTimerStop -= OnCooldownEnd;
        }

        public void Fire(Vector2 direction)
        {
            SpawnBullet(direction, transform.position);
            _currentBulletIndex = (_currentBulletIndex + 1) % _evolutionsList.Length;
            IsOnCooldown = true;
            _cooldownCountdown.Reset(GetFireRate());
            _cooldownCountdown.Start();
            _wwiseEventHandler.PostAudioEvent(ProjectileSFX.Fired);
        }
        
        private void SpawnBullet(Vector3 velocity, Vector2 position)
        {
            ProjectileDataSO evolutionData = GetEvolutionIfAny();
            Projectile projectile = _projectileFactory.CreateProjectile(_baseProjectile);
            projectile.SetUp(position, velocity.normalized, _baseProjectileData, evolutionData, _generalWeaponMod);
        }
        
        private ProjectileDataSO GetEvolutionIfAny()
        {
            return _evolutionsList[_currentBulletIndex].IsEvolution ? _evolutionsList[_currentBulletIndex] : null;
        }
        
        public bool CanAddBulletEvolution()
        {
            return _evolutionsList.Any(x => !x.IsEvolution);
        }
        
        public bool CanAddBulletEvolution(ProjectileType type)
        {
            return _evolutionsList.Any(x => !x.IsEvolution) && !_evolutionsList.Any(x => x.GetProjectileType() == type);
        }
        
        public void AddBulletEvolution(ProjectileType type, ProjectileDataSO newEvolutionData)
        {
            if (!CanAddBulletEvolution(type))
            {
                Debug.LogError("Attempted to add bullet evolution but no slots available or already contains type.");
                return;
            }
    
            int index = Array.FindIndex(_evolutionsList, x => !x.IsEvolution);
            if (index == -1)
            {
                Debug.LogError("No unevolved slots found despite earlier check.");
                return;
            }

            _evolutionsList[index] = newEvolutionData;
            UpdatePlayerHUD();
        }
        
        public bool CanUpgradeEvolution(ProjectileType type)
        {
            return _database.TryGetProjectileData(type, _bulletLevels[type] + 1, out _);
        }
        
        public bool HasBulletEvolution(ProjectileType type)
        {
            return _evolutionsList.Any(x => x.GetProjectileType() == type && x.IsEvolution);
        }
        
        public bool CanUpgradeAnyEvolution()
        {
            return _evolutionsList.Any(x => x.IsEvolution && _database.TryGetProjectileData(x.GetProjectileType(), _bulletLevels[x.GetProjectileType()] + 1, out _));
        }
        
        public void UpgradeEvolutions(ProjectileType type, ProjectileDataSO newEvolutionData)
        {
            if (!_database.TryGetProjectileData(type, _bulletLevels[type] + 1, out _))
            {
                Debug.LogError("Max level reached for projectile type or evolution data not found.");
                return;
            }
            
            _bulletLevels[type] += 1;

            for (int i = 0; i < _evolutionsList.Length; i++)
            {
                if (_evolutionsList[i].GetProjectileType() == type)
                {
                    _evolutionsList[i] = newEvolutionData;
                }
            }
                
            UpdatePlayerHUD();
        }
        
        public void UpgradeWeapon(WeaponUpgradeSO upgradeData)
        {
            upgradeData.ApplyUpgrade(_generalWeaponMod);
        }
        
        private void UpdatePlayerHUD()
        {
            List<Color> colors = new ();
            List<int> levels = new ();
            
            foreach (ProjectileDataSO evolution in _evolutionsList)
            {
                colors.Add(evolution.Color);
                levels.Add(_bulletLevels[evolution.GetProjectileType()]);
            }
            
            _signalBus.Fire(new PlayerBulletsChangedSignal(colors, levels));
        }
        
        private void OnCooldownEnd() => IsOnCooldown = false;

        
        private float GetFireRate()
        {
            float fireRate = (_fireRate + _generalWeaponMod.IncreasedFireRate) * _generalWeaponMod.FireRateMultiplier;
            return fireRate < 0f ? 0f : fireRate;
        }

        public int GetProjectileLevel(ProjectileType type)
        {
            return _bulletLevels.TryGetValue(type, out int level) ? level : 0;
        }
    }
}
