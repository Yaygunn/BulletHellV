using BH.Runtime.Entities;
using BH.Runtime.Input;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Factories
{
    public class AIFactory : IAIFactory
    {
        private AIMeleePool _meleePool;
        private AIRangedPool _rangedPool;
        
        public AIFactory(AIMeleePool meleePool, AIRangedPool rangedPool)
        {
            _meleePool = meleePool;
            _rangedPool = rangedPool;
        }

        public AIMeleeController CreateAIMelee()
        {
            AIMeleeController aiMelee = _meleePool.Spawn();
            aiMelee.SetPool(_meleePool);
            return aiMelee;
        }
        
        public AIRangedController CreateAIRanged()
        {
            AIRangedController aiRanged = _rangedPool.Spawn();
            aiRanged.SetPool(_rangedPool);
            return aiRanged;
        }
    }
}