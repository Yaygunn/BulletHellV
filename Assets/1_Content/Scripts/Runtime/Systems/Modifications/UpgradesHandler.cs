using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Entities;
using BH.Runtime.Managers;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Scripts.Runtime.UI;
using BH.Utilities.Extensions;
using GH.Scriptables;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Systems
{
    public enum UpgradeType
    {
        AddBullet,
        UpgradeBullet,
        UpgradeWeapon,
        UpgradePlayer
    }

    public class UpgradesHandler : IInitializable, IDisposable
    {
        private SignalBus _signalBus;
        private ILevelStateHandler _levelStateHandler;
        private LevelManager _levelManager;
        private DatabaseSO _database;

        private List<UpgradeOption> _generatedChoices;

        public UpgradesHandler(SignalBus signalBus, ILevelStateHandler levelStateHandler, DatabaseSO database,
            LevelManager levelManager)
        {
            _signalBus = signalBus;
            _levelStateHandler = levelStateHandler;
            _database = database;
            _levelManager = levelManager;
        }

        public void Initialize()
        {
            _levelStateHandler.OnLevelStateChanged += OnLevelStateChanged;
            _signalBus.Subscribe<UpgradeSelectedSignal>(OnUpgradeSelected);
        }

        public void Dispose()
        {
            _levelStateHandler.OnLevelStateChanged -= OnLevelStateChanged;
            _signalBus.TryUnsubscribe<UpgradeSelectedSignal>(OnUpgradeSelected);
        }

        private void OnLevelStateChanged(LevelState levelState)
        {
            if (levelState != LevelState.Upgrading)
                return;

            _levelManager.TogglePause(true);
            _generatedChoices = GenerateUpgradeOptions();
            _signalBus.Fire(new UpgradesShowSignal(_generatedChoices));
            TogglePlayerUIInput(true);
        }

        private List<UpgradeOption> GenerateUpgradeOptions()
        {
            List<UpgradeOption> options = new();
            HashSet<string> usedDescriptions = new();

            while (options.Count < 3)
            {
                UpgradeType upgradeType = GetRandomUpgradeType();
                ProjectileType projectileType = GetRandomValidProjectileType(upgradeType);

                if (projectileType is ProjectileType.PlayerBasicBullet or ProjectileType.EnemyBasicBullet)
                {
                    Debug.Log("No valid projectile types found.");
                    continue;
                }

                string tracker;
                WeaponUpgradeSO weaponUpgrade = null;
                StatUpgradeSO statUpgrade = null;
                ProjectileDataSO projectileData = null;

                switch (upgradeType)
                {
                    case UpgradeType.AddBullet:
                        projectileData = GetProjectileEvolutionData(projectileType);
                        tracker = $"{projectileData.ProjectileName}";
                        break;
                    case UpgradeType.UpgradeBullet:
                        projectileData = GetNextProjectileEvolutionData(projectileType);
                        tracker = $"{projectileData.ProjectileName}";
                        break;
                    case UpgradeType.UpgradeWeapon:
                        weaponUpgrade = GetRandomWeaponUpgrade();
                        tracker = $"{weaponUpgrade.UpgradeName}";
                        break;
                    case UpgradeType.UpgradePlayer:
                        statUpgrade = GetRandomStatUpgrade();
                        tracker = $"{statUpgrade.UpgradeName}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (!usedDescriptions.Contains(tracker))
                {
                    options.Add(new UpgradeOption(upgradeType, projectileType, projectileData, weaponUpgrade, statUpgrade));
                    usedDescriptions.Add(tracker);
                }
            }

            return options;
        }

        private void OnUpgradeSelected(UpgradeSelectedSignal signal)
        {
            UpgradeOption selectedUpgrade = _generatedChoices[signal.UpgradeIndex];
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            Stats playerStats = _levelManager.Player.Stats;

            switch (selectedUpgrade.Type)
            {
                case UpgradeType.AddBullet:
                    playerWeapon.AddBulletEvolution(selectedUpgrade.ProjectileType, selectedUpgrade.ProjectileData);
                    break;
                case UpgradeType.UpgradeBullet:
                    playerWeapon.UpgradeEvolutions(selectedUpgrade.ProjectileType, selectedUpgrade.ProjectileData);
                    break;
                case UpgradeType.UpgradeWeapon:
                    playerWeapon.UpgradeWeapon(selectedUpgrade.WeaponUpgrade);
                    break;
                case UpgradeType.UpgradePlayer:
                    playerStats.ModifyStat(selectedUpgrade.StatUpgrade);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _levelManager.TogglePause(false);
            _levelStateHandler.SetLevelState(LevelState.NormalRound);
            TogglePlayerUIInput(false);
        }

        private UpgradeType GetRandomUpgradeType()
        {
            List<UpgradeType> validUpgradeTypes = new()
            {
                UpgradeType.UpgradeWeapon,
                UpgradeType.UpgradePlayer
            };

            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanAddBulletEvolution())
            {
                validUpgradeTypes.Add(UpgradeType.AddBullet);
            }
            if (playerWeapon.CanUpgradeAnyEvolution())
            {
                validUpgradeTypes.Add(UpgradeType.UpgradeBullet);
            }

            return validUpgradeTypes[UnityEngine.Random.Range(0, validUpgradeTypes.Count)];
        }

        private ProjectileType GetRandomValidProjectileType(UpgradeType upgradeType)
        {
            List<ProjectileType> validTypes = Enum.GetValues(typeof(ProjectileType))
                .Cast<ProjectileType>()
                .Where(t => t != ProjectileType.EnemyBasicBullet && t != ProjectileType.PlayerBasicBullet)
                .ToList();

            WeaponComponent playerWeapon = _levelManager.Player.Weapon;

            if (upgradeType == UpgradeType.AddBullet)
            {
                validTypes = validTypes.Where(type => playerWeapon.CanAddBulletEvolution(type)).ToList();
            }
            else if (upgradeType == UpgradeType.UpgradeBullet)
            {
                validTypes = validTypes.Where(type => playerWeapon.HasBulletEvolution(type) && playerWeapon.CanUpgradeEvolution(type)).ToList();
            }

            if (validTypes.Count == 0)
            {
                Debug.Log("WE RAN OUT OF PROJECTILE ADD/UPGRADE...");
                return ProjectileType.PlayerBasicBullet;
            }

            validTypes.Shuffle();
            return validTypes.First();
        }

        private ProjectileDataSO GetProjectileEvolutionData(ProjectileType type)
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            int currentLevel = playerWeapon.GetProjectileLevel(type);
            _database.TryGetProjectileData(type, currentLevel, out ProjectileDataSO projectileData);
            return projectileData;
        }

        private ProjectileDataSO GetNextProjectileEvolutionData(ProjectileType type)
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            int currentLevel = playerWeapon.GetProjectileLevel(type);
            _database.TryGetProjectileData(type, currentLevel + 1, out ProjectileDataSO projectileData);
            return projectileData;
        }

        private WeaponUpgradeSO GetRandomWeaponUpgrade()
        {
            WeaponUpgradeSO[] upgrades = _database.WeaponUpgradeData.ToArray();
            int randomIndex = UnityEngine.Random.Range(0, upgrades.Length);
            return upgrades[randomIndex];
        }

        private StatUpgradeSO GetRandomStatUpgrade()
        {
            StatUpgradeSO[] stats = _database.StatUpgradeData.ToArray();
            int randomIndex = UnityEngine.Random.Range(0, stats.Length);
            return stats[randomIndex];
        }
        
        private void TogglePlayerUIInput(bool enable)
        {
            if (enable)
            {
                _levelManager.Player.InputProvider.EnableUIControls();
            }
            else
            {
                _levelManager.Player.InputProvider.EnablePlayerControls();
            }
        }
    }
}
