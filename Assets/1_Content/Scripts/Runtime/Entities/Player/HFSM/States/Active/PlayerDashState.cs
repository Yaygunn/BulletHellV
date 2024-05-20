using BH.Runtime.Audio;
using BH.Runtime.StateMachines;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class PlayerDashState : PlayerActiveState
    {
        public PlayerDashState(PlayerController player, StateMachine<PlayerState> stateMachine) : base(player, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _player.StateName = "Dash";
            
            _player.Dash.DashCompletedEvent += OnDashCompleted;

            _player.WwiseEventHandler.PostAudioEvent(PlayerSFX.Dash, _player.gameObject);
            _player.Stats.SetInvincibility(true);
            _player.Dash.StartDash(GetDashDirection());
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
            _player.Stats.SetInvincibility(false);
            _player.Dash.DashCompletedEvent -= OnDashCompleted;
            
            base.Exit();
        }
        
        private Vector2 GetMouseWorldPosition()
        {
            Vector2 mousePosition = _player.InputProvider.MousePosition;
            Vector2 worldMousePosition =  Camera.main.ScreenToWorldPoint(mousePosition);
            return worldMousePosition;
        }
        
        private Vector2 GetDashDirection()
        {
            if (_player.InputProvider.MoveInput != Vector2.zero)
                return _player.InputProvider.MoveInput.normalized;
            
            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            Vector2 dashDireciton = (mouseWorldPosition - (Vector2)_player.transform.position).normalized;
            
            return dashDireciton;
        }
        
        private void OnDashCompleted()
        {
            _stateMachine.ChangeState(_player.PlayerHFSM.PreviousState);
        }
    }
}