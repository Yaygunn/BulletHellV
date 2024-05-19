using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIBusyState : BossAIState
    {
        public BossAIBusyState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}