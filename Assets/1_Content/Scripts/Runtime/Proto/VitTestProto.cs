using System;
using System.Collections;
using BH.Runtime.Entities;
using BH.Runtime.Managers;
using BH.Runtime.Scenes;
using BH.Runtime.Systems;
using BH.Scriptables.Databases;
using BH.Scriptables.Scenes;
using BH.Utilities;
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
        [Button]
        private void TestUpgradesShow()
        {
            _levelStateHandler.SetLevelState(LevelState.Upgrading);
        }
        
        [Inject]
        private LevelManager _levelManager;
        [SerializeField]
        private ProjectileType _projectileType;
        
        [Button]
        private void TestAddBullet()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanAddBulletEvolution())
            {
                playerWeapon.AddBulletEvolution(_projectileType);
            }
        }
        
        [Button]
        private void TestUpgradeBullet()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            if (playerWeapon.CanUpgradeEvolution(_projectileType))
            {
                playerWeapon.UpgradeEvolutions(_projectileType);
            }
        }
        
        [Inject]
        private DatabaseSO _database;
        [Button]
        private void TestUpgradeWeapon()
        {
            WeaponComponent playerWeapon = _levelManager.Player.Weapon;
            WeaponUpgradeSO upgrade = GetRandomUpgrade();
            playerWeapon.UpgradeWeapon(upgrade);
        }
        
        private WeaponUpgradeSO GetRandomUpgrade()
        {
            WeaponUpgradeSO[] upgrades = _database.WeaponUpgradeData.ToArray();
            
            int randomIndex = UnityEngine.Random.Range(0, upgrades.Length);
            return upgrades[randomIndex];
        }
        

        
        // [Inject]
        // private GameManager _gameManager;
        //
        // [Inject]
        // private SceneLoader _sceneLoader;
        
        // [Inject]
        // private SignalBus _signalBus;
        //
        // private void OnEnable()
        // {
        //     _signalBus.Subscribe<TestSpawnSignal>(OnTestSpawnSignal);
        // }
        //
        // private void OnDisable()
        // {
        //     _signalBus.Unsubscribe<TestSpawnSignal>(OnTestSpawnSignal);
        // }
        //
        // private void OnTestSpawnSignal()
        // {
        //     Debug.Log("Projectile Spawned!");
        // }
    }
}