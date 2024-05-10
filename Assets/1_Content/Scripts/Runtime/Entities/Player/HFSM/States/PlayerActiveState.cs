using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class PlayerActiveState : PlayerState
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
            
            // TODO: We should avoid polling each update frame...
            _player.Movement.Move(_player.Direction);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}