using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Systems
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        public Vector2 MoveValue { get; private set; }
        public Vector2 LookValue { get; private set; }
        public event Action JumpEvent;
        public event Action AttackEvent;
        public event Action CrouchEvent;
        public event Action InteractionEvent;
        public event Action SummonEvent;
        private InputSystem_Actions _inputActions;

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookValue = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackEvent?.Invoke();
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            CrouchEvent?.Invoke();
        }

        public void OnNext(InputAction.CallbackContext context) { }

        public void OnPrevious(InputAction.CallbackContext context) { }

        public void OnSprint(InputAction.CallbackContext context) { }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractionEvent?.Invoke();
            }
        }

        public void OnSummon(InputAction.CallbackContext context)
        {
            if (context.performed)
                SummonEvent?.Invoke();
        }

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputSystem_Actions();
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
        }
    }
}
