using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIMoveTwoState : BossAIActiveTwoState
    {
        public BossAIMoveTwoState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}