using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class MeleeAIBusyState : MeleeAIState
    {
        public MeleeAIBusyState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _meleeAI.StateName = "Busy";
        }
    }
}