using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Scripts.Controllers
{
    public class InputReader : MonoBehaviour
    {
        #region Statements

        public Vector2 MoveValue { get; private set; }
        public Vector2 LookValue { get; private set; }
        public bool RunValue { get; set; }
        
        public Action JumpEvent { get; set; }
        public Action RollEvent { get; set; }
        
        public Action StopAnimationEvent { get; set; }

        #endregion

        #region Events

        public void OnMove(InputValue value) => MoveValue = value.Get<Vector2>();
        public void OnLook(InputValue value) => LookValue = value.Get<Vector2>();
        
        public void OnRun() => RunValue = !RunValue;
        public void OnJump() => JumpEvent?.Invoke();

        public void OnRoll()
        {
            if (MoveValue.Equals(Vector2.zero)) return;
            
            RollEvent?.Invoke();
        }
        
        public void StopAnimation() => StopAnimationEvent?.Invoke();

        #endregion
    }
}
