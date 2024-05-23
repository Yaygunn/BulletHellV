using BH.Runtime.Entities;

namespace BH.Runtime.Factories
{
    public interface IBossFactory
    {
        public AIBossController CreateBoss();
    }
}