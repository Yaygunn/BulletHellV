using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIMoveThreeState : BossAIActiveThreeState
    {
        public BossAIMoveThreeState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}