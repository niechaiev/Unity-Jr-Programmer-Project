using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputReader : MonoBehaviour, PlayerInput.IPlayerActions
    {
        public Vector2 MoveComposite;
        public Action<InputAction.CallbackContext> OnSelectPerformed;
        public Action<InputAction.CallbackContext> OnOrderPerformed;
        public Action<InputAction.CallbackContext> OnZoomCameraPerformed;

        private PlayerInput playerInput;


        private void Start()
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
            playerInput.Player.Enable();
            ;
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSelectPerformed?.Invoke(context);
        }

        public void OnOrder(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnOrderPerformed?.Invoke(context);
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            MoveComposite = context.ReadValue<Vector2>();
        }

        public void OnZoomCamera(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnZoomCameraPerformed?.Invoke(context);
        }
    }
}