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
        private GameObject _projectilePrefab;
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
            Container.BindMemoryPool<Projectile, ProjectilePool>()
                .WithInitialSize(_initialPoolSize)
                .FromComponentInNewPrefab(_projectilePrefab)
                .UnderTransformGroup("Projectiles");
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
            
            // Other
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(_levelSettings).NonLazy();
        }
    }
}