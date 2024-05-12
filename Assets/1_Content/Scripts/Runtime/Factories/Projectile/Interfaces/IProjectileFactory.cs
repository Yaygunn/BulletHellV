using BH.Runtime.Systems;

namespace BH.Runtime.Factories
{
    public interface IProjectileFactory
    {
        public Projectile CreateProjectile(ProjectileType type);
    }
}