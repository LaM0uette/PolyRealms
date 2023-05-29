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
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent -= OnJump;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;

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

        #endregion
    }
}
