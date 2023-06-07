using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        #region Statements

        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsTransitioning) return;

            if (HasAnimationReachedStage(.2f) && StateMachine.IsGrounded())
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            
            if (StateMachine.Velocity.y < StateMachine.Landing && !StateMachine.IsGrounded()) 
                StateMachine.SwitchState(new PlayerFallState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity = new Vector3(StateMachine.Velocity.x, StateMachine.JumpForce, StateMachine.Velocity.z);
            
            SetCapsuleSize(StateMachine.JumpCapsuleHeight, StateMachine.InitialCapsuleRadius);

            StateMachine.TransitionToAnimation(PlayerAnimationIds.Jump);
        }

        public override void Tick(float deltaTime)
        {
            var speed = GetMoveSpeed();
            Move(speed);
            
            ApplyGravity();
            
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
            ResetCapsuleSize();
        }

        #endregion
    }
}