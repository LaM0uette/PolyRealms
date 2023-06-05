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

            if (StateMachine.Velocity.y > StateMachine.MaxLanding)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
            else
                StateMachine.SwitchState(new PlayerLandingState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.TransitionToAnimation(PlayerAnimationIds.Fall, .4f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();

            var speed = GetMoveSpeed() * StateMachine.AirSpeedModifier;
            Move(speed* StateMachine.AirSpeedModifier);
            
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