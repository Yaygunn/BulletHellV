using BH.Runtime.Entities;
using Zenject;

namespace BH.Runtime.Factories
{
    public class AIMeleePool : MonoMemoryPool<AIMeleeController>
    {
        protected override void OnCreated(AIMeleeController meleeAI)
        {
            base.OnCreated(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
        
        protected override void OnSpawned(AIMeleeController meleeAI)
        {
            base.OnSpawned(meleeAI);
            meleeAI.gameObject.SetActive(true);
        }
        
        protected override void OnDespawned(AIMeleeController meleeAI)
        {
            base.OnDespawned(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
    }
}