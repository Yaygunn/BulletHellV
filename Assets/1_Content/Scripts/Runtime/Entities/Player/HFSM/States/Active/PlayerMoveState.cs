﻿using BH.Runtime.StateMachines;
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
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            // TODO: We should avoid polling each update frame...
            _player.Movement.Move(_player.InputProvider.MoveInput);
            
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
        }
        
        private bool ShouldIdle()
        {
            return _player.InputProvider.MoveInput == Vector2.zero;
        }
    }
}