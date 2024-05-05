using BH.Runtime.Systems;
using Zenject;

namespace BH.Runtime.Factories
{
    public class ProjectilePool : MonoMemoryPool<Projectile>
    {
        protected override void OnCreated(Projectile projectile)
        {
            base.OnCreated(projectile);
            projectile.gameObject.SetActive(false);
        }
        
        protected override void OnSpawned(Projectile projectile)
        {
            base.OnSpawned(projectile);
            projectile.gameObject.SetActive(true);
        }
        
        protected override void OnDespawned(Projectile projectile)
        {
            base.OnDespawned(projectile);
            projectile.gameObject.SetActive(false);
        }
    }
}