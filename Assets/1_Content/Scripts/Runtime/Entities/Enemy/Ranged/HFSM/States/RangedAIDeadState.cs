using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class RangedAIDeadState : RangedAIState
    {
        public RangedAIDeadState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
    }
}