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
        public bool WalkValue  { get; set; }
        public bool RunValue  { get; set; }
        public bool CrouchValue  { get; set; }
        
        public Action JumpEvent { get; set; }
        public Action RollEvent { get; set; }
        public Action CrouchActionEvent { get; set; }
        public Action SlideEvent { get; set; }
        
        public Action DashForwardEvent { get; set; }
        public Action DashBackwardEvent { get; set; }
        public Action DashLeftEvent { get; set; }
        public Action DashRightEvent { get; set; }
        
        public Action StopAnimationEvent { get; set; }

        #endregion

        #region Events

        public void OnMove(InputValue value) => MoveValue = value.Get<Vector2>();
        public void OnLook(InputValue value) => LookValue = value.Get<Vector2>();

        public void OnWalk() => WalkValue = !WalkValue;
        public void OnRun() => RunValue = !RunValue;
        public void OnCrouch()
        {
            if (!RunValue || MoveValue.Equals(Vector2.zero))
            {
                CrouchValue = !CrouchValue; 
                return;
            }
            
            if (CrouchValue && (WalkValue || RunValue)) CrouchValue = !CrouchValue;
        }
        
        public void OnJump() => JumpEvent?.Invoke();
        public void OnRoll() => RollEvent?.Invoke();
        public void OnCrouchAction() => CrouchActionEvent?.Invoke();
        public void OnSlide() { if (RunValue) SlideEvent?.Invoke(); }
        
        public void OnDashForward() => DashForwardEvent?.Invoke();
        public void OnDashBackward() => DashBackwardEvent?.Invoke();
        public void OnDashLeft() => DashLeftEvent?.Invoke();
        public void OnDashRight() => DashRightEvent?.Invoke();
        
        public void StopAnimation() => StopAnimationEvent?.Invoke();

        #endregion
    }
}
