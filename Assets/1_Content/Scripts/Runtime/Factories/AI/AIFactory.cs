using BH.Runtime.Entities;
using BH.Runtime.Input;
using UnityEngine;
using Zenject;

namespace BH.Runtime.Factories
{
    public class AIFactory : IAIFactory
    {
        private AIMeleePool _meleePool;
        
        public AIFactory(AIMeleePool meleePool)
        {
            _meleePool = meleePool;
        }

        public AIMeleeController CreateAIMelee()
        {
            AIMeleeController aiMelee = _meleePool.Spawn();
            aiMelee.SetPool(_meleePool);
            return aiMelee;
        }
    }
}