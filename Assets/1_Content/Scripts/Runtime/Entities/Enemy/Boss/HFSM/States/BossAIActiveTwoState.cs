using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIActiveTwoState : BossAIState
    {
        public BossAIActiveTwoState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}