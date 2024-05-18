using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities.States.Active
{
    public class RangedAIShootState : RangedAIActiveState
    {
        public RangedAIShootState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
    }
}