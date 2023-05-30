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
            else if (StateMachine.Inputs.CrouchValue) StateMachine.SwitchState(new PlayerCrouchState(StateMachine));
        }

        private (float speed, float animationValue) GetSpeed()
        {
            if (StateMachine.Inputs.WalkValue) return (StateMachine.WalkSpeed, -1f);
            if (StateMachine.Inputs.RunValue) return (StateMachine.RunSpeed, 2f);
            
            return StateMachine.IsMoving() ? (StateMachine.NormalSpeed, 1f) : (0, 0);
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.MoveBlendTree, .1f);

            SubscribeEvents();
        }

        public override void Tick(float deltaTime)
        {
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
        }
        
        private void OnJump()
        {
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, 0);
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

//StateMachine.Inputs.RunValue = false;
//StateMachine.Inputs.WalkValue = false;