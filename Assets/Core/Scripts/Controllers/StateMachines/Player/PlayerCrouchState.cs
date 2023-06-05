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
            if (StateMachine.Velocity.y < 0 && !StateMachine.IsGrounded()) StateMachine.SwitchState(new PlayerFallState(StateMachine));
            else if (!StateMachine.Inputs.CrouchValue && !ForceCrouchByHeight()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();

            SetCapsuleSize(StateMachine.CrouchCapsuleHeight, StateMachine.InitialCapsuleRadius);
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.CrouchBlendTree, .2f);

        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            
            var (speed, _) = GetSpeed();
            Move(speed * StateMachine.CrouchSpeedModifier);

            var spd = !StateMachine.IsMoving() ? 0 : .6f;
            if (StateMachine.Inputs.WalkValue) spd = .3f;
            if (StateMachine.Inputs.RunValue) spd = 1.5f;
            
            AnimatorSetFloat(PlayerAnimationIds.Speed, spd, .1f);
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, !StateMachine.IsMoving() ? 0 : 1f, .1f);
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