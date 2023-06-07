using Cinemachine;
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
        protected float TargetSpeed;
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
        
        protected void ApplyGravity(float multiplier = 2f)
        {
            if (StateMachine.Velocity.y > StateMachine.Gravity)
            {
                StateMachine.Velocity.y += Physics.gravity.y * multiplier * Time.deltaTime;
            }
        }

        protected float GetMoveSpeed()
        {
            if (!StateMachine.IsMoving()) return 0;
            if (StateMachine.Inputs.WalkValue) return StateMachine.WalkSpeed;
            if (StateMachine.Inputs.RunValue) return StateMachine.RunSpeed;
            
            return StateMachine.NormalSpeed;
        }
        
        protected bool HasAnimationReachedStage(float value)
        {
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            return normalizedTime > value;
        }

        protected void Move(float targetSpeed)
        {
            if (!StateMachine.IsMoving()) targetSpeed = 0;

            var controllerVelocity = StateMachine.Controller.velocity;
            var currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0, controllerVelocity.z).magnitude;

            if (currentHorizontalSpeed < targetSpeed - OFFSET || currentHorizontalSpeed > targetSpeed + OFFSET)
            {
                TargetSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * 10f);
                TargetSpeed = Mathf.Round(TargetSpeed * 1000f) / 1000f;
            }
            else TargetSpeed = targetSpeed;

            MoveRotation();
            
            var targetDirection = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;
            
            StateMachine.Controller.Move(targetDirection.normalized * (TargetSpeed * Time.deltaTime) + new Vector3(0, StateMachine.Velocity.y, 0) * Time.deltaTime);
        }

        protected void MoveRotation(float multiplier = 1f)
        {
            var inputDirection = new Vector3(StateMachine.Inputs.MoveValue.x, 0, StateMachine.Inputs.MoveValue.y).normalized;
            if (!StateMachine.IsMoving()) return;

            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + StateMachine.MainCamera.transform.eulerAngles.y;
            var rotation = Mathf.LerpAngle(StateMachine.transform.eulerAngles.y, _targetRotation, OFFSET * multiplier);
            StateMachine.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        
        protected bool ForceCrouchByHeight()
        {
            if (!Physics.SphereCast(StateMachine.transform.position, StateMachine.InitialCapsuleRadius, Vector3.up, out var hit,
                    StateMachine.InitialCapsuleHeight, -1, QueryTriggerInteraction.Ignore)) return false;
            
            return hit.point.y - StateMachine.transform.position.y > StateMachine.CrouchCapsuleHeight;
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

        protected void CameraZoom()
        {
            if (StateMachine.Inputs.CameraZoomValue.Equals(0)) return;

            foreach (var camera in StateMachine.cinemachineCameras)
            {
                var thirdPersonFollow = camera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                thirdPersonFollow.CameraDistance += StateMachine.Inputs.CameraZoomValue * StateMachine.ZoomForce * Time.deltaTime;
                
                if (thirdPersonFollow.CameraDistance <= StateMachine.MinZoom) thirdPersonFollow.CameraDistance = StateMachine.MinZoom;
                if (thirdPersonFollow.CameraDistance >= StateMachine.MaxZoom) thirdPersonFollow.CameraDistance = StateMachine.MaxZoom;
            }
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
	    
        protected float GetCapsuleHeight()
        {
            return StateMachine.Controller.height;
        }
        
        protected void SetCapsuleSize(float newHeight, float newRadius, float offsetY = 0)
        {
            StateMachine.Controller.height = newHeight;
            StateMachine.Controller.center = new Vector3(0, newHeight * 0.5f + offsetY, 0);

            if (newRadius > newHeight * 0.5f)
                newRadius = newHeight * 0.5f;

            StateMachine.Controller.radius = newRadius;
        }
        
        protected void SetRootMotion(bool value)
        {
            StateMachine.UseRootMotion = value;
            StateMachine.Animator.applyRootMotion = value;
        }

        protected void ResetVelocity()
        {
            StateMachine.Velocity = Vector3.zero;
        }

        #endregion
    }
}
