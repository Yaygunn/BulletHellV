using BH.Runtime.Factories;
using BH.Runtime.Systems;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _projectilePrefab;
        [SerializeField]
        private int _initialPoolSize = 50;
        
        public override void InstallBindings()
        {
            Container.BindMemoryPool<Projectile, ProjectilePool>()
                .WithInitialSize(_initialPoolSize)
                .FromComponentInNewPrefab(_projectilePrefab)
                .UnderTransformGroup("Projectiles");
            
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
        }
    }
}