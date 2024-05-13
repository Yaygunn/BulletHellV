using UnityEngine;

namespace BH.Runtime.Input
{
    public interface IInputProvider
    {
        // Vector2 Values
        public Vector2 MoveInput { get; }
        public Vector2 MousePosition { get; }
        
        // Buttons
        public InputState FireInput { get;}
        public InputState DashInput { get;}

        // Methods
        public void EnablePlayerControls();
        public void EnableUIControls();
    }
}