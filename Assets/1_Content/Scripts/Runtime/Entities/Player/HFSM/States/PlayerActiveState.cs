using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public abstract class PlayerActiveState : PlayerState
    {
        public PlayerActiveState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldDash())
            {
                _stateMachine.ChangeState(_player.DashState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        private bool ShouldDash()
        {
            if (_player.PlayerHFSM.CurrentState == _player.DashState)
                return false;
            
            if (_player.Dash.IsOnCooldown)
                return false;
            
            return _player.InputProvider.DashInput.Pressed;
        }
    }
}