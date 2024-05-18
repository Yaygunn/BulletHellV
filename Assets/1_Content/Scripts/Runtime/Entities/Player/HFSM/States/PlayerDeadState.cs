using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class PlayerDeadState : PlayerState
    {
        private float _deadTimer;
        private float _deadDuration;
        
        public PlayerDeadState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.StateName = "Dead";
            _player.Animator.SetTrigger(_player.AnimatorParams.IsDeadTrigger);
            
            _deadDuration = _player.AnimatorParams.IsDeadDuration;
            _deadTimer = 0f;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            _deadTimer += Time.deltaTime;
            
            if (_deadTimer >= _deadDuration)
            {
                _player.HandlePlayerDeath();
            }
        }
    }
}