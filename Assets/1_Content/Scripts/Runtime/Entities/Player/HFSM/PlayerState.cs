using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public abstract class PlayerState : BaseState<PlayerState>
    {
        protected PlayerController _player;
        
        public PlayerState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(stateMachine)
        {
            _player = player;
        }
    }
}