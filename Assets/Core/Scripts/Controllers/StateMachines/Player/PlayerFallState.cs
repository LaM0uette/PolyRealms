using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Functions

        private void CheckStateChange()
        {
            if (!StateMachine.IsGrounded()) return;
            
            StateMachine.SwitchState(new PlayerLandingState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Fall, .4f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();

            var speed = GetMoveSpeed() * StateMachine.AirSpeedModifier;
            if (StateMachine.Velocity.y < StateMachine.MaxHardLanding) speed /= 3f;
            Move(speed);
            
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}