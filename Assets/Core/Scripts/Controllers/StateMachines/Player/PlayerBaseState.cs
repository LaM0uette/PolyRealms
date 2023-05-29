using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;

        // Move
        private float _speed;
        private float _targetRotation;
        private float _verticalVelocity;
        
        // Rotation
        private static float _cinemachineTargetYaw;
        private static float _cinemachineTargetPitch;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        #region Functions
        
        protected void Move()
        {
            var targetSpeed = StateMachine.WalkSpeed;
            const float offset = 0.1f;
            
            if (!StateMachine.IsMoving()) targetSpeed = 0.0f;

            var controllerVelocity = StateMachine.Controller.velocity;
            var currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0.0f, controllerVelocity.z).magnitude;

            if (currentHorizontalSpeed < targetSpeed - offset || currentHorizontalSpeed > targetSpeed + offset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * 10f);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else _speed = targetSpeed;

            var inputDirection = new Vector3(StateMachine.Inputs.MoveValue.x, 0.0f, StateMachine.Inputs.MoveValue.y).normalized;

            if (!StateMachine.Inputs.MoveValue.Equals(Vector2.zero))
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + StateMachine.MainCamera.transform.eulerAngles.y;
                var rotation = Mathf.LerpAngle(StateMachine.transform.eulerAngles.y, _targetRotation, offset);
                StateMachine.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _verticalVelocity += StateMachine.Gravity * Time.deltaTime;
            
            StateMachine.Controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            StateMachine.Animator.SetFloat(PlayerAnimationIds.LocomotionSpeed, targetSpeed, offset, Time.deltaTime);
        }

        protected void CameraRotation()
        {
            var deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += StateMachine.Inputs.LookValue.x * deltaTimeMultiplier * StateMachine.MouseSensitivity * 10;
            _cinemachineTargetPitch += StateMachine.Inputs.LookValue.y * deltaTimeMultiplier * StateMachine.MouseSensitivity * 10;

            _cinemachineTargetYaw = _cinemachineTargetYaw.ClampAngle(float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = _cinemachineTargetPitch.ClampAngle(StateMachine.BottomClamp, StateMachine.TopClamp);

            StateMachine._cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        #endregion
    }
}
