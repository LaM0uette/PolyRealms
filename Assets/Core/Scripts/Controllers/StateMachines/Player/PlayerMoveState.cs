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
            StateMachine.Inputs.CrouchActionEvent += OnCrouchAction;
            StateMachine.Inputs.SlideEvent += OnSlide;
            StateMachine.Inputs.DashForwardEvent += OnDashForward;
            StateMachine.Inputs.DashBackwardEvent += OnDashBackward;
            StateMachine.Inputs.DashLeftEvent += OnDashLeft;
            StateMachine.Inputs.DashRightEvent += OnDashRight;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent -= OnJump;
            StateMachine.Inputs.RollEvent -= OnRoll;
            StateMachine.Inputs.CrouchActionEvent -= OnCrouchAction;
            StateMachine.Inputs.SlideEvent -= OnSlide;
            StateMachine.Inputs.DashForwardEvent -= OnDashForward;
            StateMachine.Inputs.DashBackwardEvent -= OnDashBackward;
            StateMachine.Inputs.DashLeftEvent -= OnDashLeft;
            StateMachine.Inputs.DashRightEvent -= OnDashRight;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (!StateMachine.IsGrounded()) 
                StateMachine.SwitchState(new PlayerFallState(StateMachine));
            
            if (StateMachine.Inputs.CrouchValue && StateMachine.IsGrounded()) 
                StateMachine.SwitchState(new PlayerCrouchState(StateMachine));
        }

        private void ChangeStateDash(PlayerDashState.DashDirection dashDirection)
        {
            StateMachine.SwitchState(new PlayerDashState(StateMachine, dashDirection));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.MoveBlendTree, .2f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            CheckStateChange();
            
            var (speed, animationValue) = GetSpeed();
            
            Move(speed);
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, animationValue, .1f);
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
            ResetVelocity();
            
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, 0);
        }
        
        #endregion

        #region InputEvents

        private void OnJump() => StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        
        private void OnRoll() => StateMachine.SwitchState(new PlayerRollState(StateMachine));
        
        private void OnCrouchAction()
        {
            if (!StateMachine.IsMoving()) return;
            
            OnRoll();
        }
        
        private void OnSlide() => StateMachine.SwitchState(new PlayerSlideState(StateMachine));
        
        private void OnDashForward() =>  ChangeStateDash(PlayerDashState.DashDirection.Forward);
        private void OnDashBackward() => ChangeStateDash(PlayerDashState.DashDirection.Backward);
        private void OnDashLeft() => ChangeStateDash(PlayerDashState.DashDirection.Left);
        private void OnDashRight() => ChangeStateDash(PlayerDashState.DashDirection.Right);

        #endregion
    }
}
