using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIShootOneState : BossAIActiveOneState
    {
        public BossAIShootOneState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}