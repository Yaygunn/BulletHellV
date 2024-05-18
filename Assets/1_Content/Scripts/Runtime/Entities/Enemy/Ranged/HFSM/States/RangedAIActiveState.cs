using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities.States
{
    public class RangedAIActiveState : RangedAIState
    {
        public RangedAIActiveState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
    }
}