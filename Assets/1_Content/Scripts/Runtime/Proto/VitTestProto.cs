using System;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Entities;
using BH.Runtime.Managers;
using BH.Runtime.Systems;
using BH.Scriptables;
using BH.Scriptables.Databases;
using BH.Utilities.Extensions;
using GH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Test
{
    public class VitTestProto : MonoBehaviour
    {
        [Inject]
        private ILevelStateHandler _levelStateHandler;
        
        [Inject]
        private LevelManager _levelManager;

        [Inject]
        private DatabaseSO _database;

        [SerializeField]
        private ProjectileType _projectileSelection = ProjectileType.HomingBullet;
        
        [TitleGroup("Selected"), Button(ButtonSizes.Large)]
        private void AddSelectedProjectile()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanAddBulletEvolution(_projectileSelection))
            {
                var projectileData = GetNextProjectileEvolutionData(_projectileSelection);
                playerWeapon.AddBulletEvolution(_projectileSelection, projectileData);
            }
        }
        
        [TitleGroup("Selected"), Button(ButtonSizes.Large)]
        private void UpgradeSelectedProjectile()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanUpgradeEvolution(_projectileSelection))
            {
                var projectileData = GetNextProjectileEvolutionData(_projectileSelection);
                playerWeapon.UpgradeEvolutions(_projectileSelection, projectileData);
            }
        }
        
        [TitleGroup("Random"), Button(ButtonSizes.Large)]
        private void AddRandomProjectile()
        {
            List<ProjectileType> projectileTypes = Enum.GetValues(typeof(ProjectileType))
                .Cast<ProjectileType>()
                .Where(t => t != ProjectileType.EnemyBasicBullet && t != ProjectileType.PlayerBasicBullet)
                .ToList();
    
            projectileTypes.Shuffle();

            WeaponComponent playerWeapon = _levelManager.Player.Weapon;

            foreach (ProjectileType type in projectileTypes)
            {
                if (playerWeapon.CanAddBulletEvolution(type))
                {
                    var projectileData = GetNextProjectileEvolutionData(type);
                    playerWeapon.AddBulletEvolution(type, projectileData);
                    break;
                }
            }
        }
        
        [TitleGroup("Random"), Button(ButtonSizes.Large)]
        private void UpgradeRandomProjectile()
        {
            List<ProjectileType> projectileTypes = Enum.GetValues(typeof(ProjectileType))
                .Cast<ProjectileType>()
                .Where(t => t != ProjectileType.EnemyBasicBullet && t != ProjectileType.PlayerBasicBullet)
                .ToList();
    
            projectileTypes.Shuffle();

            WeaponComponent playerWeapon = _levelManager.Player.Weapon;

            foreach (ProjectileType type in projectileTypes)
            {
                if (playerWeapon.CanUpgradeEvolution(type))
                {
                    var projectileData = GetNextProjectileEvolutionData(type);
                    playerWeapon.UpgradeEvolutions(type, projectileData);
                    break;
                }
            }
        }
        
        [TitleGroup("Random"), Button(ButtonSizes.Large)]
        private void RandomWeaponUpgrade()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            WeaponUpgradeSO upgrade = GetRandomWeaponUpgrade();
            playerWeapon.UpgradeWeapon(upgrade);
        }
        
        private WeaponUpgradeSO GetRandomWeaponUpgrade()
        {
            WeaponUpgradeSO[] upgrades = _database.WeaponUpgradeData.ToArray();
            
            int randomIndex = UnityEngine.Random.Range(0, upgrades.Length);
            return upgrades[randomIndex];
        }
        
        [TitleGroup("Random"), Button(ButtonSizes.Large)]
        private void RandomStatUpgrade()
        {
            Stats stats = _levelManager.Player.Stats;
            StatUpgradeSO upgrade = GetRandomStatUpgrade();
            stats.ModifyStat(upgrade);
        }

        private StatUpgradeSO GetRandomStatUpgrade()
        {
            StatUpgradeSO[] stats = _database.StatUpgradeData.ToArray();
            
            int randomIndex = UnityEngine.Random.Range(0, stats.Length);
            return stats[randomIndex];
        }

        private ProjectileDataSO GetNextProjectileEvolutionData(ProjectileType type)
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            int currentLevel = playerWeapon.GetProjectileLevel(type);
            _database.TryGetProjectileData(type, currentLevel + 1, out ProjectileDataSO projectileData);
            return projectileData;
        }
    }
}
