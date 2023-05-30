using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerCrouchState : PlayerBaseState
    {
        #region Statements

        public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent += OnJump;
            StateMachine.Inputs.RollEvent += OnRoll;
            StateMachine.Inputs.CrouchActionEvent += OnCrouchAction;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent -= OnJump;
            StateMachine.Inputs.RollEvent -= OnRoll;
            StateMachine.Inputs.CrouchActionEvent -= OnCrouchAction;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.Velocity.y < 0 && !StateMachine.IsGround()) StateMachine.SwitchState(new PlayerFallState(StateMachine));
            else if (!StateMachine.Inputs.CrouchValue) StateMachine.SwitchState(new PlayerIdleState(StateMachine));
        }

        private (float speed, float animationValue) GetSpeed()
        {
            if (StateMachine.Inputs.RunValue) return (StateMachine.RunSpeed, 2f);
            if (StateMachine.Inputs.WalkValue) return (StateMachine.WalkSpeed, -1f);

            return (StateMachine.NormalSpeed, 1f);
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.CrouchBlendTree, .1f);

            SubscribeEvents();
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            
            var (speed, animationValue) = GetSpeed();
            
            Move(speed * 0.75f);
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
        
        private void OnCrouchAction()
        {
            if (!StateMachine.IsMoving()) return;
            
            OnRoll();
        }

        #endregion
    }
}