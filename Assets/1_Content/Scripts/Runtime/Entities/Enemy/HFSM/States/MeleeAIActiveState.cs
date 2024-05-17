using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class MeleeAIActiveState : MeleeAIState
    {
        public MeleeAIActiveState(AIMeleeController meleeAI, StateMachine<MeleeAIState> stateMachine) : base(meleeAI, stateMachine)
        {
            
        }
    }
}