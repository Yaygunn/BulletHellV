using BH.Runtime.StateMachines;

namespace BH.Runtime.Entities
{
    public class PlayerBusyState : PlayerState
    {
        public PlayerBusyState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
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