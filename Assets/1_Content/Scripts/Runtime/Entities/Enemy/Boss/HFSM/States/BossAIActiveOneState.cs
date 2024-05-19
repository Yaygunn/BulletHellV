using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIActiveOneState : BossAIState
    {
        public BossAIActiveOneState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}

