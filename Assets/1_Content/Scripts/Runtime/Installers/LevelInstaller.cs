using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Runtime.Systems;
using BH.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [BoxGroup("Level Settings"), SerializeField]
        private LevelSettingsSO _levelSettings;
        
        [BoxGroup("Player"), SerializeField, Tooltip("Name of the Player Prefab asset file in the resources folder.")]
        private string _playerObjectName = "Player";
        
        [BoxGroup("Projectiles"), SerializeField]
        private int _initialPoolSize = 50;
        
        public override void InstallBindings()
        {
            // Player
            GameObject playerPrefab = Resources.Load<GameObject>(_playerObjectName);
            if (playerPrefab == null)
            {
                Debug.LogError($"Console prefab '{_playerObjectName}' not found in a resources folder.");
                return;
            }
            Container.Bind<IPLayerFactory>().To<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
            
            // Projectiles
            BindProjectilePools();
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
            
            // Upgrades
            Container.BindInterfacesAndSelfTo<UpgradesHandler>().AsSingle();
            
            // Other
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(_levelSettings).NonLazy();
        }
        
        private void BindProjectilePools()
        {
            foreach (ProjectileType type in System.Enum.GetValues(typeof(ProjectileType)))
            {
                GameObject prefab = Resources.Load<GameObject>($"Projectiles/{type}");
                if (prefab == null)
                {
                    Debug.LogError($"Projectile prefab for {type} not found in Resources/Projectiles/");
                    continue;
                }
                Container.BindMemoryPool<Projectile, ProjectilePool>()
                    .WithId(type)
                    .WithInitialSize(_initialPoolSize)
                    .FromComponentInNewPrefab(prefab)
                    .UnderTransformGroup($"{type}Pool")
                    .AsCached()
                    .NonLazy();
            }
        }
    }
}