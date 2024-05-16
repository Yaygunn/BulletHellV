using BH.Runtime.StateMachines;
using BH.Runtime.Systems;
using UnityEngine;

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
            
            FaceMouse();
            
            if (ShouldDash())
            {
                _stateMachine.ChangeState(_player.DashState);
            }
            else if (ShouldShoot())
            {
                _player.Weapon.Fire(GetMouseDirection());
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
        
        private bool ShouldShoot()
        {
            if (_player.PlayerHFSM.CurrentState == _player.DashState)
                return false;
            
            if (_player.Weapon.IsOnCooldown)
                return false;
            
            return _player.InputProvider.FireInput.Pressed;
        }
        
        private void FaceMouse()
        {
            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            Vector2 direction = (mouseWorldPosition - (Vector2)_player.transform.position).normalized;
            _player.FlipCharacter(direction.x > 0);
        }
        
        protected Vector2 GetMouseWorldPosition()
        {
            Vector2 mousePosition = _player.InputProvider.MousePosition;
            Vector2 worldMousePosition =  _player.Camera.ScreenToWorldPoint(mousePosition);
            return worldMousePosition;
        }
        
        protected Vector2 GetMouseDirection()
        {
            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            Vector2 shootDirection = (mouseWorldPosition - (Vector2)_player.transform.position).normalized;
            
            return shootDirection;
        }
    }
}