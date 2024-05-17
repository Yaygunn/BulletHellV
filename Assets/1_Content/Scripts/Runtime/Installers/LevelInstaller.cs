using BH.Runtime.Entities;
using BH.Runtime.Factories;
using BH.Runtime.Managers;
using BH.Runtime.Systems;
using BH.Scriptables;
using DP.Utilities;
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
            if (Tools.TryLoadResource(_playerObjectName, out GameObject playerPrefab))
            {
                Container.Bind<IPLayerFactory>().To<PlayerFactory>().AsSingle().WithArguments(playerPrefab);
            }
            
            // Enemies
            if (Tools.TryLoadResource($"Enemies/AIMelee", out GameObject aiMeleePrefab))
            {
                Container.BindMemoryPool<AIMeleeController, AIMeleePool>()
                    .WithInitialSize(20)
                    .FromComponentInNewPrefab(aiMeleePrefab)
                    .UnderTransformGroup("Enemies")
                    .AsCached()
                    .NonLazy();
                Container.Bind<IAIFactory>().To<AIFactory>().AsSingle();
            }
            
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
                if (Tools.TryLoadResource($"Projectiles/{type}", out GameObject prefab))
                {
                    Container.BindMemoryPool<Projectile, ProjectilePool>()
                        .WithId(type)
                        .WithInitialSize(_initialPoolSize)
                        .FromComponentInNewPrefab(prefab)
                        .UnderTransformGroup("ProjectilePools")
                        .AsCached()
                        .NonLazy();
                }
            }
        }
    }
}