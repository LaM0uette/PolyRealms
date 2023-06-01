using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        #region Statements

        protected readonly PlayerStateMachine StateMachine;

        protected const float OFFSET = .1f;
        
        // Move
        protected float _speed;
        private float _targetRotation;

        // Rotation
        private static float _cinemachineTargetYaw;
        private static float _cinemachineTargetPitch;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        #endregion
        
        #region Functions
        
        protected void ApplyGravity()
        {
            if (StateMachine.Velocity.y > Physics.gravity.y)
            {
                StateMachine.Velocity.y += Physics.gravity.y * 1.4f * Time.deltaTime;
            }
        }
        
        protected (float speed, float animationValue) GetSpeed()
        {
            if (!StateMachine.IsMoving()) return StateMachine.Inputs.CrouchValue ? (0, -1f) : (0, 0);
            
            if (StateMachine.Inputs.WalkValue) return StateMachine.Inputs.CrouchValue ? (StateMachine.WalkSpeed, 0) : (StateMachine.WalkSpeed, -1f);
            if (StateMachine.Inputs.RunValue) return (StateMachine.RunSpeed, 2f);
            
            return (StateMachine.NormalSpeed, 1f);
        }

        protected void Move(float targetSpeed)
        {
            if (!StateMachine.IsMoving()) targetSpeed = 0;

            var controllerVelocity = StateMachine.Controller.velocity;
            var currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0, controllerVelocity.z).magnitude;

            if (currentHorizontalSpeed < targetSpeed - OFFSET || currentHorizontalSpeed > targetSpeed + OFFSET)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * 10f);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else _speed = targetSpeed;

            MoveRotation();
            
            var targetDirection = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;
            StateMachine.Controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0, StateMachine.Velocity.y, 0) * Time.deltaTime);
        }

        protected void MoveRotation()
        {
            var inputDirection = new Vector3(StateMachine.Inputs.MoveValue.x, 0, StateMachine.Inputs.MoveValue.y).normalized;
            if (!StateMachine.IsMoving()) return;
            
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + StateMachine.MainCamera.transform.eulerAngles.y;
            var rotation = Mathf.LerpAngle(StateMachine.transform.eulerAngles.y, _targetRotation, OFFSET);
            StateMachine.transform.rotation = Quaternion.Euler(0, rotation, 0);
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

        protected void AnimatorSetFloat(int id, float value)
        {
            StateMachine.Animator.SetFloat(id, value);
        }
        
        protected void AnimatorSetFloat(int id, float value, float dampTime)
        {
            StateMachine.Animator.SetFloat(id, value, dampTime, Time.deltaTime);
        }
        
        protected void ResetCapsuleSize()
        {
            SetCapsuleSize(StateMachine.InitialCapsuleHeight, StateMachine.InitialCapsuleRadius);
        }
	    
        protected void SetCapsuleSize(float newHeight, float newRadius, float offsetY = 0)
        {
            StateMachine.Controller.height = newHeight;
            StateMachine.Controller.center = new Vector3(0, newHeight * 0.5f + offsetY, 0);

            if (newRadius > newHeight * 0.5f)
                newRadius = newHeight * 0.5f;

            StateMachine.Controller.radius = newRadius;
        }
        
        protected void SetControllerEnable(bool value)
        {
            StateMachine.Controller.enabled = value;
        }

        #endregion
    }
}
