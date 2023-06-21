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
        public float CameraZoomValue { get; private set; }
        public bool WalkValue  { get; set; }
        public bool RunValue  { get; set; }
        public bool CrouchValue  { get; set; }
        
        public Action JumpEvent { get; set; }
        public Action RollJumpEvent { get; set; }
        public Action RollEvent { get; set; }
        public Action CrouchActionEvent { get; set; }
        public Action SlideEvent { get; set; }
        public Action DashEvent { get; set; }
        public Action SwitchCameraEvent { get; set; }
        
        public Action StopAnimationEvent { get; set; }

        #endregion

        #region Events

        public void OnMove(InputValue value) => MoveValue = value.Get<Vector2>();
        public void OnLook(InputValue value) => LookValue = value.Get<Vector2>();

        public void OnCameraZoom(InputValue value)
        {
            var zoomValue = value.Get<float>();
            CameraZoomValue = zoomValue.Equals(0) ? 0 : zoomValue;
        }

        public void OnWalk()
        {
            WalkValue = !WalkValue;
            if (WalkValue) RunValue = false;
        }
        public void OnRun()
        {
            RunValue = !RunValue;
            if (RunValue) WalkValue = false;
        }
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
        public void OnRollJump() => RollJumpEvent?.Invoke();
        public void OnRoll() => RollEvent?.Invoke();
        public void OnCrouchAction() => CrouchActionEvent?.Invoke();
        public void OnSlide() { if (RunValue) SlideEvent?.Invoke(); }
        public void OnDash() => DashEvent?.Invoke();
        public void OnSwitchCamera() => SwitchCameraEvent?.Invoke();
        
        public void StopAnimation() => StopAnimationEvent?.Invoke();

        #endregion
    }
}
