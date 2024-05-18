using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities.States
{
    public class RangedAIBusyState : RangedAIState
    {
        public RangedAIBusyState(AIRangedController rangedController, StateMachine<RangedAIState> stateMachine) : base(rangedController, stateMachine)
        {
        }
    }
}