using Sirenix.OdinInspector;
using UnityEngine;

namespace BH.Runtime.Input
{
    public class InputHandler : MonoBehaviour, IInputProvider
    {
        private PlayerInput _inputActions;

        #region Player Inputs

        // Vector2 Values
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly]
        public Vector2 MoveInput { get; private set; }
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly]
        public Vector2 MousePosition { get; private set; }
        
        // Buttons
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly, InlineProperty]
        public InputState FireInput { get; private set; }
        [field: FoldoutGroup("Debugging"), SerializeField, ReadOnly, InlineProperty]
        public InputState DashInput { get; private set; }
        
        #endregion

        private void Awake()
        {
            _inputActions = new PlayerInput();

            FireInput = new InputState(_inputActions.Player.Fire);
            DashInput = new InputState(_inputActions.Player.Dash);
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();
        }

        private void Update()
        {
            MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            MousePosition = _inputActions.Player.MousePosition.ReadValue<Vector2>();
        }

        private void LateUpdate()
        {
            FireInput.Reset();
            DashInput.Reset();
        }
        
        public void EnablePlayerControls()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Enable();
        }
        
        public void EnableUIControls()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
    }
}