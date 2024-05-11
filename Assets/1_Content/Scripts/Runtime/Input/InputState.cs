using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BH.Runtime.Input
{
    /// <summary>
    /// Object to create different input states for buttons (Pressed, Held, Released)
    /// and register them to an InputAction listener.
    /// </summary>
    [Serializable]
    public class InputState
    {
        [field: BoxGroup("States"), SerializeField, ReadOnly]
        public bool Pressed { get; private set; }
        [field: BoxGroup("States"), SerializeField, ReadOnly]
        public bool Held { get; private set; }
        [field: BoxGroup("States"), SerializeField, ReadOnly]
        public bool Released { get; private set; }

        public InputAction InputAction { get; private set; }
        public InputAction AlterantiveInputAction { get; private set; }
        
        public InputState() { }
        
        public InputState(InputAction inputAction)
        {
            InputAction = inputAction;
            AlterantiveInputAction = inputAction;
            
            inputAction.started += ctx => OnStarted();
            inputAction.performed += ctx => OnPerformed();
            inputAction.canceled += ctx => OnCanceled();
        }
        
        public InputState(InputAction inputAction, InputAction inputAction2)
        {
            InputAction = inputAction;
            AlterantiveInputAction = inputAction2;
            
            inputAction.started += ctx => OnStarted();
            inputAction.performed += ctx => OnPerformed();
            inputAction.canceled += ctx => OnCanceled();
            
            inputAction2.started += ctx => OnStarted();
            inputAction2.performed += ctx => OnPerformed();
            inputAction2.canceled += ctx => OnCanceled();
        }

        public void OnStarted() => Held = true;

        public void OnPerformed() => Pressed = true;

        public void OnCanceled()
        {
            Held = false;
            Pressed = false;
            Released = true;
        }

        public void Reset()
        {
            Released = false;
        }
    }
}