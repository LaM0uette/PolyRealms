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
        private InputAction _runAction;
        private InputAction _crouchAction;
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
            _runAction = _inputActions.FindAction("Run");
            _crouchAction = _inputActions.FindAction("Crouch");
            _attackAction = _inputActions.FindAction("Attack");
        }

        #endregion

        #region Subscriptions

        private void SubscribeActions()
        {
            _walkAction.started += OnWalkTrue;
            _walkAction.canceled += OnWalkFalse;
            
            _runAction.started += OnRunTrue;
            _runAction.canceled += OnRunFalse;
            
            _crouchAction.started += OnCrouchTrue;
            _crouchAction.canceled += OnCrouchFalse;
            
            _attackAction.started += OnAttackTrue;
            _attackAction.canceled += OnAttackFalse;
            
            _inputActions.Enable();
            _walkAction.Enable();
            _runAction.Enable();
            _crouchAction.Enable();
            _attackAction.Enable();
        }
        
        private void UnsubscribeActions()
        {
            _walkAction.started -= OnWalkTrue;
            _walkAction.canceled -= OnWalkFalse;
            
            _runAction.started -= OnRunTrue;
            _runAction.canceled -= OnRunFalse;
            
            _crouchAction.started -= OnCrouchTrue;
            _crouchAction.canceled -= OnCrouchFalse;
            
            _attackAction.started -= OnAttackTrue;
            _attackAction.canceled -= OnAttackFalse;

            _inputActions.Disable();
            _walkAction.Disable();
            _runAction.Disable();
            _crouchAction.Disable();
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
        
        private void OnWalkTrue(InputAction.CallbackContext _)
        {
            WalkValue = true;
            RunValue = false;
        }
        private void OnWalkFalse(InputAction.CallbackContext _)
        {
            WalkValue = false;
        }
        private void OnRunTrue(InputAction.CallbackContext _)
        {
            RunValue = true;
            WalkValue = false;
        }
        private void OnRunFalse(InputAction.CallbackContext _)
        {
            RunValue = false;
        }
        private void OnCrouchTrue(InputAction.CallbackContext _)
        {
            if (RunValue && !MoveValue.Equals(Vector2.zero)) return;
            
            CrouchValue = true;
        }
        private void OnCrouchFalse(InputAction.CallbackContext _)
        {
            CrouchValue = false;
        }
        private void OnAttackTrue(InputAction.CallbackContext _)
        {
            AttackValue = true;
            WalkValue = false;
            RunValue = false;
            CrouchValue = false;
        }
        private void OnAttackFalse(InputAction.CallbackContext _)
        {
            AttackValue = false;
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
