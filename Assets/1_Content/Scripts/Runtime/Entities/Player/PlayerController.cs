using System;
using BH.Runtime.Test;
using UnityEngine;

namespace BH.Runtime.Entities
{
    /// <summary>
    /// This PlayerController class will be more properly set up.. just using for testing...
    /// </summary>
    public class PlayerController : Entity
    {
        private Vector2 _direction;
        
        private MovementComponent _movement;

        private void Awake()
        {
            _movement = GetComponent<MovementComponent>();
        }

        private void Update()
        {
            // TODO: This will be redone with the new input system.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            _direction = new Vector2(horizontal, vertical).normalized;
            _movement.Move(_direction);
        }
    }
}