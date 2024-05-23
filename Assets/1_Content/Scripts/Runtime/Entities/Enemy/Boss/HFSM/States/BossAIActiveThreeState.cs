using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIActiveThreeState : BossAIState
    {
        public BossAIActiveThreeState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}