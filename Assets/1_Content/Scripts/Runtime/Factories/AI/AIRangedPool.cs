using BH.Runtime.Entities;
using Zenject;

namespace BH.Runtime.Factories
{
    public class AIRangedPool : MonoMemoryPool<AIRangedController>
    {
        protected override void OnCreated(AIRangedController meleeAI)
        {
            base.OnCreated(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
        
        protected override void OnSpawned(AIRangedController meleeAI)
        {
            base.OnSpawned(meleeAI);
            meleeAI.gameObject.SetActive(true);
        }
        
        protected override void OnDespawned(AIRangedController meleeAI)
        {
            base.OnDespawned(meleeAI);
            meleeAI.gameObject.SetActive(false);
        }
    }
}