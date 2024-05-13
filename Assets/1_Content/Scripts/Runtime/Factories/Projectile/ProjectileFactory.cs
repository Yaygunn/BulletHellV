using BH.Runtime.Systems;
using Zenject;

namespace BH.Runtime.Factories
{
    public class ProjectileFactory : IProjectileFactory
    {
        private DiContainer _container;
        
        public ProjectileFactory(DiContainer container)
        {
            _container = container;
        }
        
        public Projectile CreateProjectile(ProjectileType type)
        {
            ProjectilePool pool = _container.ResolveId<ProjectilePool>(type);
            Projectile projectile = pool.Spawn();
            projectile.SetPool(pool);
            return projectile;
        }
    }
}