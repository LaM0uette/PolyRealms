using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerRollJumpState : PlayerBaseState
    {
        public PlayerRollJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Functions

        private void Roll(float speed)
        {
            var controllerVelocity = StateMachine.Controller.velocity;
            var currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0, controllerVelocity.z).magnitude;
            var targetDirection = StateMachine.transform.forward;
            
            if (currentHorizontalSpeed < speed - OFFSET || currentHorizontalSpeed > speed + OFFSET)
            {
                TargetSpeed = Mathf.Lerp(currentHorizontalSpeed, speed, Time.deltaTime * 10f);
                TargetSpeed = Mathf.Round(TargetSpeed * 1000f) / 1000f;
            }
            else TargetSpeed = speed;

            StateMachine.Controller.Move(targetDirection.normalized * (TargetSpeed * Time.deltaTime) + new Vector3(0, StateMachine.Velocity.y, 0) * Time.deltaTime);
        }
        
        private void CheckStateChange()
        {
            if (StateMachine.IsTransitioning) return;

            if (HasAnimationReachedStage(.8f) && StateMachine.IsGrounded())
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }
        
        #endregion
        
        #region Events

        public override void Enter()
        {
            SetCapsuleSize(StateMachine.RollCapsuleHeight, StateMachine.InitialCapsuleRadius);

            StateMachine.TransitionToAnimation(PlayerAnimationIds.RollJump);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            
            var speed = GetMoveSpeed() + StateMachine.RollSpeed;
            Roll(speed);
            
            MoveRotation();
            
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}