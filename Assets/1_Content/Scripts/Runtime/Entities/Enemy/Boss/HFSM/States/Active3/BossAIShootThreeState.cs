using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class BossAIShootThreeState : BossAIActiveThreeState
    {
        public BossAIShootThreeState(AIBossController controller, StateMachine<BossAIState> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}