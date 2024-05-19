using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIShootTwoState : BossAIActiveTwoState
    {
        public BossAIShootTwoState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}