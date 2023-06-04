using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerSlideState : PlayerBaseState
    {
        public PlayerSlideState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.CrouchActionEvent += OnCrouchAction;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.CrouchActionEvent -= OnCrouchAction;
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
            SetCapsuleSize(StateMachine.SlideCapsuleHeight, StateMachine.InitialCapsuleRadius);

            SetRootMotion(true);
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Slide, .2f);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.Inputs.MoveValue.y < 0) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            
            MoveRotation(.1f);
            
            if (IsAnimationInTransition()) return;
                
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);
                
            if (normalizedTime > 0.95f)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
            SetRootMotion(false);
            ResetCapsuleSize();
        }

        private void OnCrouchAction()
        {
            if (!StateMachine.IsMoving()) return;
            
            StateMachine.SwitchState(new PlayerRollState(StateMachine));
        }

        #endregion
    }
}