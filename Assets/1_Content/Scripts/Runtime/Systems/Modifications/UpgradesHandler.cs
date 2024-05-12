using System;
using System.Collections.Generic;
using BH.Runtime.Managers;
using BH.Scriptables.Databases;
using BH.Scripts.Runtime.UI;
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
        private DatabaseSO _database;

        private List<UpgradeOption> _generatedChoices;

        public UpgradesHandler(SignalBus signalBus, ILevelStateHandler levelStateHandler, DatabaseSO database)
        {
            _signalBus = signalBus;
            _levelStateHandler = levelStateHandler;
            _database = database;
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

            _generatedChoices = GenerateUpgradeOptions();
            _signalBus.Fire(new UpgradesShowSignal(_generatedChoices));
        }

        private List<UpgradeOption> GenerateUpgradeOptions()
        {
            List<UpgradeOption> options = new();
            for (int i = 0; i < 3; i++)
            {
                UpgradeType upgradeType = GetRandomUpgradeType();
                ProjectileType projectileType = GetRandomProjectileType();
                string description = upgradeType switch
                {
                    UpgradeType.AddBullet => $"Add {projectileType} Bullet",
                    UpgradeType.UpgradeBullet => $"Upgrade {projectileType} Bullet",
                    _ => throw new ArgumentOutOfRangeException()
                };
                options.Add(new UpgradeOption(upgradeType, projectileType, description));
            }
            return options;
        }

        private void OnUpgradeSelected(UpgradeSelectedSignal signal)
        {
            UpgradeOption selectedUpgrade = _generatedChoices[signal.UpgradeIndex];
            switch (selectedUpgrade.Type)
            {
                case UpgradeType.AddBullet:
                    break;
                case UpgradeType.UpgradeBullet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private UpgradeType GetRandomUpgradeType()
        {
            return (UpgradeType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(UpgradeType)).Length);
        }

        private ProjectileType GetRandomProjectileType()
        {
            // TODO: exclude enemybasicbullet and playerbasicbullet
            return (ProjectileType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ProjectileType)).Length);
        }
    }
}