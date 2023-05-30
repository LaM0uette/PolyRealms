using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        #region Statements
        
        private float _initialVelocityY;

        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.Velocity.y < _initialVelocityY && !StateMachine.IsGround()) StateMachine.SwitchState(new PlayerFallState(StateMachine));
            else if (StateMachine.Velocity.y < 0 && StateMachine.IsGround()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SetCapsuleSize(1.2f, StateMachine.InitialCapsuleRadius, .5f);
            
            _initialVelocityY = -StateMachine.JumpForce;
            StateMachine.Velocity = new Vector3(StateMachine.Velocity.x, StateMachine.JumpForce, StateMachine.Velocity.z);
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Jump, .2f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            CheckStateChange();

            var speed = StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.NormalSpeed;
            Move(speed);
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