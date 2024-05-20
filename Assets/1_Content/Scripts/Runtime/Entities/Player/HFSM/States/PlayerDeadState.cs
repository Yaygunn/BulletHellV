using BH.Runtime.Audio;
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
            
            _player.WwiseEventHandler.PostAudioEvent(PlayerSFX.Die, _player.gameObject);
            _player.Collider.enabled = false;
            _player.ModelRenderer.enabled = false;
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

        public override void Exit()
        {
            base.Exit();
            
            _player.Collider.enabled = true;
            _player.ModelRenderer.enabled = true;
        }
    }
}