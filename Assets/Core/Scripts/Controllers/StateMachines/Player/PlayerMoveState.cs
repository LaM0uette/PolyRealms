using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        #region Statements

        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent += OnJump;
            StateMachine.Inputs.RollEvent += OnRoll;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent -= OnJump;
            StateMachine.Inputs.RollEvent -= OnRoll;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.Velocity.y < 0 && !StateMachine.IsGround()) StateMachine.SwitchState(new PlayerFallState(StateMachine));
            else if (!StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerIdleState(StateMachine));
        }

        private (float speed, float animationValue) GetSpeed()
        {
            return StateMachine.Inputs.RunValue ? (StateMachine.RunSpeed, 2f) : (StateMachine.WalkSpeed, 1f);
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.LocomotionBlendTree, .1f);

            SubscribeEvents();
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            
            var (speed, animationValue) = GetSpeed();
            
            Move(speed);
            AnimatorSetFloat(PlayerAnimationIds.LocomotionSpeed, animationValue, .1f);
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
        }
        
        private void OnJump()
        {
            AnimatorSetFloat(PlayerAnimationIds.LocomotionSpeed, 0);
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }
        
        private void OnRoll()
        {
            StateMachine.SwitchState(new PlayerRollState(StateMachine));
        }

        #endregion
    }
}