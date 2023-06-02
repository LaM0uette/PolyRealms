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
            else if (!StateMachine.Inputs.CrouchValue) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
            
            SetCapsuleSize(1.2f, StateMachine.InitialCapsuleRadius);
            
            StateMachine.Velocity.y = Physics.gravity.y;
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.CrouchBlendTree, .2f);

        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            
            var (speed, animationValue) = GetSpeed();
            
            Move(speed * 0.6f);
            
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, animationValue, .1f);
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
            ResetCapsuleSize();
        }
        
        private void OnJump()
        {
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, 0);
            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
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