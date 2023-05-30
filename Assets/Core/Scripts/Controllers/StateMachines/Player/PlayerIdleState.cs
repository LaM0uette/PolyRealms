using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        #region Statements

        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
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
            else if (StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
            StateMachine.Inputs.RunValue = false;
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.LocomotionBlendTree, .2f);

            SubscribeEvents();
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            Move();
            
            AnimatorSetFloat(PlayerAnimationIds.LocomotionSpeed, 0, .1f);
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
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }

        private void OnRoll()
        {
            StateMachine.SwitchState(new PlayerRollState(StateMachine));
        }

        #endregion
    }
}
