using BH.Runtime.Systems;
using Zenject;

namespace BH.Runtime.Factories
{
    public class ProjectilePool : MonoMemoryPool<Projectile>
    {
        protected override void OnCreated(Projectile meleeAI)
        {
            base.OnCreated(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
        
        protected override void OnSpawned(Projectile meleeAI)
        {
            base.OnSpawned(meleeAI);
            meleeAI.gameObject.SetActive(true);
        }
        
        protected override void OnDespawned(Projectile meleeAI)
        {
            base.OnDespawned(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
    }
}