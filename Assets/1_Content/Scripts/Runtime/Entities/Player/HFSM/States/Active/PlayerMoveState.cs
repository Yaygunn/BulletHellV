using BH.Runtime.Audio;
using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class PlayerMoveState : PlayerActiveState
    {
        public PlayerMoveState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.StateName = "Move";
            _player.Animator.SetBool(_player.AnimatorParams.IsMovingBool, true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            // TODO: We should avoid polling each update frame...
            _player.Movement.Move(_player.InputProvider.MoveInput, _player.Stats.CurrentSpeed);
            
            //VerifyFacingDirection();
            
            if (ShouldIdle())
            {
                _stateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            
            _player.Animator.SetBool(_player.AnimatorParams.IsMovingBool, false);
            _player.Movement.Stop();
        }
        
        private bool ShouldIdle()
        {
            return _player.InputProvider.MoveInput == Vector2.zero;
        }
        
        private void VerifyFacingDirection()
        {
            switch (_player.InputProvider.MoveInput.x)
            {
                case > 0 when !_player.IsFacingRight:
                    _player.FlipCharacter(true);
                    break;
                case < 0 when _player.IsFacingRight:
                    _player.FlipCharacter(false);
                    break;
            }
        }
    }
}