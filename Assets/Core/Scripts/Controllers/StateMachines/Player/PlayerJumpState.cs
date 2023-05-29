using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
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
            StateMachine.Velocity = new Vector3(StateMachine.Velocity.x, StateMachine.JumpForce, StateMachine.Velocity.z);

            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Jump, .1f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            Move();
            
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        #endregion
    }
}