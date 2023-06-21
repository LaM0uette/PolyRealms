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
        
        //InputActions
        private InputActionAsset _inputActions;
        private InputAction _walkAction;
        private InputAction _attackAction;
        
        public bool WalkValue  { get; set; }
        public bool RunValue  { get; set; }
        public bool CrouchValue  { get; set; }
        public bool AttackValue  { get; set; }
        
        public Action JumpEvent { get; set; }
        public Action RollJumpEvent { get; set; }
        public Action RollEvent { get; set; }
        public Action CrouchActionEvent { get; set; }
        public Action SlideEvent { get; set; }
        public Action DashEvent { get; set; }
        public Action SwitchCameraEvent { get; set; }
        
        public Action StopAnimationEvent { get; set; }
        
        private void Awake()
        {
            _inputActions = GetComponent<PlayerInput>().actions;
            InitializeInputActions();
        }
        private void InitializeInputActions()
        {
            _walkAction = _inputActions.FindAction("Walk");
            _attackAction = _inputActions.FindAction("Attack");
        }

        #endregion

        #region Subscriptions

        private void SubscribeActions()
        {
            _walkAction.started += OnWalkTrue;
            _walkAction.canceled += OnWalkFalse;
            
            //_attackAction.started += _ => {AttackValue = true;};
            //_attackAction.canceled += _ => {AttackValue = false;};
            
            _inputActions.Enable();
            _walkAction.Enable();
            _attackAction.Enable();
        }
        private void UnsubscribeActions()
        {
            _walkAction.started -= OnWalkTrue;
            _walkAction.canceled -= OnWalkFalse;
            
            //_attackAction.started -= _ => {AttackValue = true;};
            //_attackAction.canceled -= _ => {AttackValue = false;};
            
            _inputActions.Disable();
            _walkAction.Disable();
            _attackAction.Disable();
        }

        #endregion

        #region Events
        
        private void OnEnable()
        {
            SubscribeActions();
        }
        private void OnDisable()
        {
            UnsubscribeActions();
        }

        public void OnMove(InputValue value) => MoveValue = value.Get<Vector2>();
        public void OnLook(InputValue value) => LookValue = value.Get<Vector2>();
        public void OnCameraZoom(InputValue value)
        {
            var zoomValue = value.Get<float>();
            CameraZoomValue = zoomValue.Equals(0) ? 0 : zoomValue;
        }
        
        private void OnWalkTrue(InputAction.CallbackContext context)
        {
            WalkValue = true;
            RunValue = false;
        }
        private void OnWalkFalse(InputAction.CallbackContext context)
        {
            WalkValue = false;
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
        public void OnAttack()
        {
            AttackValue = !AttackValue;

            if (!AttackValue) return;
            WalkValue = false;
            RunValue = false;
            CrouchValue = false;
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
