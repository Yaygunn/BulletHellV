using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class RangedAIActiveState : RangedAIState
    {
        public RangedAIActiveState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
    }
}