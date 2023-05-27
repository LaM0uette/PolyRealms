using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Scripts.Controllers
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        #region Statements

        public Vector2 MoveValue { get; private set; }
        public Vector2 LookValue { get; private set; }

        #endregion

        #region Events

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookValue = context.ReadValue<Vector2>();
        }

        #endregion
    }
}
