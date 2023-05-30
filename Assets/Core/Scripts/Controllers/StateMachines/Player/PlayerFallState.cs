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
            if (StateMachine.Controller.isGrounded) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = 0;

            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Fall, .5f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            
            var speed = (StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.WalkSpeed) / 3f;
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