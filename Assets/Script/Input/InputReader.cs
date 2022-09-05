using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Game
{
    // TODO: complete InputReader
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IPlayerActions//, GameInput.IMenusActions
    {
        // Player
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction interactEvent = delegate { };

        // Menus
        // public event UnityAction<Vector2> menuMoveSelectionEvent = delegate { };
        // public event UnityAction menuConfirmEvent = delegate { };
        // public event UnityAction menuCancelEvent = delegate { };

        // !!! Remember to edit Input Reader functions upon updating the input map !!!
        private GameInput gameInput;

        private void OnEnable() {
            if (gameInput == null) {
                gameInput = new GameInput();

                gameInput.Player.SetCallbacks(this);
                // gameInput.Menus.SetCallbacks(this);
            }

            EnablePlayerInput();
        }

        private void OnDisable() {

        }

        // -----PLAYER-----
        public void OnMovement(InputAction.CallbackContext context)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                interactEvent.Invoke();
        }

        // -----MENUS-----
        // public void OnMenuMoveSelection(InputAction.CallbackContext context)
        // {
        //     menuMoveSelectionEvent.Invoke(context.ReadValue<Vector2>());
        // }

        // public void OnMenuConfirm(InputAction.CallbackContext context)
        // {
        //     if (context.phase == InputActionPhase.Performed)
        //         menuConfirmEvent.Invoke();
        // }

        // public void OnMenuCancel(InputAction.CallbackContext context)
        // {
        //     if (context.phase == InputActionPhase.Performed)
        //         menuCancelEvent.Invoke();
        // }

        // Input Reader
        public void EnablePlayerInput() {
            gameInput.Player.Enable();
        }

        // public void EnableMenusInput() {
        //     gameInput.Pointer.Disable();

        //     gameInput.Menus.Enable();
        // }

        public void DisableAllInput() {
            gameInput.Player.Disable();
        }
    }
}