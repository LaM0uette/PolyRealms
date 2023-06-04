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
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.StopAnimationEvent += StopAnimation;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.StopAnimationEvent -= StopAnimation;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            if (StateMachine.Velocity.y < 0 && !StateMachine.IsGrounded()) 
                StateMachine.SwitchState(new PlayerFallState(StateMachine));
            
            if (normalizedTime > 0.2f && StateMachine.IsGrounded())
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
            
            SetCapsuleSize(1f, StateMachine.InitialCapsuleRadius);
            
            StateMachine.Velocity = new Vector3(StateMachine.Velocity.x, StateMachine.JumpForce, StateMachine.Velocity.z);
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Jump, .1f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            
            var speed = StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.NormalSpeed;
            Move(speed);
            
            if (StateMachine.Animator.IsInTransition(0)) return;
            
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
            ResetCapsuleSize();
        }
        
        private void StopAnimation()
        {
            return;
            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            ResetCapsuleSize();
        }

        #endregion
    }
}