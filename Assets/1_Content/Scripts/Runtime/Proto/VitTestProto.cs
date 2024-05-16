using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.Runtime.Entities;
using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Runtime.Systems;
using BH.Scriptables.Databases;
using BH.Scriptables.Scenes;
using BH.Utilities;
using BH.Utilities.Extensions;
using GH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Test
{
    public class VitTestProto : MonoBehaviour
    {
        // TODO: TESTING
        
        
        [Inject]
        private ILevelStateHandler _levelStateHandler;
        // [Button]
        // private void TestUpgradesShow()
        // {
        //     _levelStateHandler.SetLevelState(LevelState.Upgrading);
        // }
        
        [Inject]
        private LevelManager _levelManager;

        [SerializeField]
        private ProjectileType _projectileSelection = ProjectileType.HomingBullet;
        
        [TitleGroup("Selected"), Button(ButtonSizes.Large)]
        private void AddSelectedProjectile()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanAddBulletEvolution())
            {
                playerWeapon.AddBulletEvolution(_projectileSelection);
            }
        }
        
        [TitleGroup("Selected"), Button(ButtonSizes.Large)]
        private void UpgradeSelectedProjectile()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanUpgradeEvolution(_projectileSelection))
            {
                playerWeapon.UpgradeEvolutions(_projectileSelection);
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
                if (playerWeapon.CanAddBulletEvolution())
                {
                    playerWeapon.AddBulletEvolution(type);
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
                    playerWeapon.UpgradeEvolutions(type);
                    break;
                }
            }
        }
        
        [Inject]
        private DatabaseSO _database;
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
    }
}