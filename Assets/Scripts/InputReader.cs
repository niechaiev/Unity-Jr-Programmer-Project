using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputReader : MonoBehaviour, PlayerInput.IPlayerActions
    {
        public Vector2 MoveComposite;
        public Action<InputAction.CallbackContext> OnSelectStarted;
        public Action<InputAction.CallbackContext> OnSelectPerformed;
        public Action<InputAction.CallbackContext> OnSelectCanceled;
        public Action<InputAction.CallbackContext> OnOrderStarted;
        public Action<InputAction.CallbackContext> OnZoomCameraStarted;
        public Action<bool> OnMultiSelectStarted;

        private PlayerInput playerInput;
        private void Start()
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
            playerInput.Player.Enable();
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            //Debug.Log(context.action);
            if (context.started)
                OnSelectStarted?.Invoke(context);
            else if (context.canceled)
                OnSelectCanceled?.Invoke(context);
        }

        public void OnOrder(InputAction.CallbackContext context)
        {
            if (context.started)
                OnOrderStarted?.Invoke(context);
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            MoveComposite = context.ReadValue<Vector2>();
        }

        public void OnZoomCamera(InputAction.CallbackContext context)
        {
            if (context.started)
                OnZoomCameraStarted?.Invoke(context);
        }

        public void OnMultiSelect(InputAction.CallbackContext context)
        {
            if (context.started)
                OnMultiSelectStarted?.Invoke(true);
            else if (context.canceled)
                OnMultiSelectStarted?.Invoke(false);
        }

        public void OnSelectDrag(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log(context);
                OnSelectPerformed?.Invoke(context);
            }

        }
    }
}