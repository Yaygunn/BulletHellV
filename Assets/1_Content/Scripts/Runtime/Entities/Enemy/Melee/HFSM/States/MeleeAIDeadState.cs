using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class MeleeAIDeadState : MeleeAIState
    {
        public MeleeAIDeadState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _meleeAI.StateName = "Dead";
        }
    }
}