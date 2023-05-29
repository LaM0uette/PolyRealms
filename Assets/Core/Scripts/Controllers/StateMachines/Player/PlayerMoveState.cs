using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        #region Statements
        
        private float _speed;
        private float _targetRotation;
        private float _verticalVelocity;

        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (!StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerIdleState(StateMachine));
        }

        private void Move()
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

        #endregion

        #region Events

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            Move();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}