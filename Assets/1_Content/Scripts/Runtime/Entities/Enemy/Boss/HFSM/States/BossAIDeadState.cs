using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIDeadState : BossAIState
    {
        public BossAIDeadState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}