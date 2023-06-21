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

        private float GetSpeedAnimation()
        {
            var spd = !StateMachine.IsMoving() ? 0 : .8f;
            if (StateMachine.Inputs.WalkValue) spd = .4f;
            if (StateMachine.Inputs.RunValue) spd = 1.5f;
            
            return spd;
        }
        
        private float GetMoveSpeedAnimation()
        {
            if (!StateMachine.IsMoving()) return 0;
            if (StateMachine.Inputs.RunValue) return 2f;
            
            return 1f;
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();

            SetCapsuleSize(StateMachine.CrouchCapsuleHeight, StateMachine.InitialCapsuleRadius);
            
            StateMachine.TransitionToAnimation(PlayerAnimationIds.CrouchBlendTree);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            
            var speed = GetMoveSpeed() * StateMachine.CrouchSpeedModifier;
            Move(speed);

            var spd = GetSpeedAnimation();
            AnimatorSetFloat(PlayerAnimationIds.Speed, spd, .1f);
            
            var animationValue = GetMoveSpeedAnimation();
            AnimatorSetFloat(PlayerAnimationIds.MoveSpeed, animationValue, .1f);
            
            CheckStateChange();
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
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