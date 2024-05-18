using BH.Runtime.Entities;

namespace BH.Runtime.Factories
{
    public interface IAIFactory
    {
        public AIMeleeController CreateAIMelee();
        public AIRangedController CreateAIRanged();
    }
}