using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerSlideState : PlayerBaseState
    {
        #region Statements

        public PlayerSlideState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion
        
        #region Functions

        private void CheckStateChange()
        {
            if (IsAnimationInTransition()) return;

            if (HasAnimationReachedStage(.9f)) 
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            SetCapsuleSize(StateMachine.SlideCapsuleHeight, StateMachine.InitialCapsuleRadius);

            SetRootMotion(true);
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Slide, .2f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity(4f);
            
            if (StateMachine.Inputs.MoveValue.y < 0) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            
            MoveRotation(.1f);
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            SetRootMotion(false);
            ResetCapsuleSize();
        }

        #endregion
    }
}