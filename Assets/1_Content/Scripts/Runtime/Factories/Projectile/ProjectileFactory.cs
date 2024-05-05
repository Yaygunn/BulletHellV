using BH.Runtime.Systems;

namespace BH.Runtime.Factories
{
    public class ProjectileFactory : IProjectileFactory
    {
        private ProjectilePool _pool;
        
        public ProjectileFactory(ProjectilePool pool)
        {
            _pool = pool;
        }
        
        public Projectile CreateProjectile()
        {
            return _pool.Spawn();
        }
    }
}