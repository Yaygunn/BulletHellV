using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class PlayerIdleState : PlayerActiveState
    {
        public PlayerIdleState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.StateName = "Idle";
            
            _player.Movement.Stop();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldMove())
            {
                _stateMachine.ChangeState(_player.MoveState);
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
        
        private bool ShouldMove()
        {
            return _player.InputProvider.MoveInput != Vector2.zero;
        }
    }
}