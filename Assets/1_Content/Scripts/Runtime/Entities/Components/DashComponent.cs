using System;
using System.Collections;
using UnityEngine;

namespace BH.Runtime.Entities
{
    public class DashComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField]
        private float _dashSpeed;
        [SerializeField]
        private float _dashDuration;
        [SerializeField]
        private float _dashCooldown;
        
        private float _currentDashCooldown;
        private float _dashTimeLeft;
        
        public bool IsOnCooldown => _currentDashCooldown > 0;
        private bool _isDashing;
        private Vector2 _dashDirection;
        
        private Rigidbody2D _rigidbody;

        public Action DashCompletedEvent;
        
        private void Awake()
        {
            // TODO: REMOVE THIS TEST
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            if (_currentDashCooldown > 0)
                _currentDashCooldown -= Time.deltaTime;

            if (!_isDashing) return;
            
            _dashTimeLeft -= Time.deltaTime;
            if (_dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        
        private void FixedUpdate()
        {
            if (!_isDashing) return;
            
            _rigidbody.velocity = _dashDirection * _dashSpeed;
        }
        
        public void StartDash(Vector2 direction)
        {
            if (!(_currentDashCooldown <= 0)) return;
            
            _dashDirection = direction;
            _currentDashCooldown = _dashCooldown;
            _dashTimeLeft = _dashDuration;
            _isDashing = true;
        }
        
        private void EndDash()
        {
            _rigidbody.velocity = Vector2.zero;
            _isDashing = false;
            DashCompletedEvent?.Invoke();
        }
    }
}