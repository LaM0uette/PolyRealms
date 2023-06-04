using Core.Scripts.StaticUtilities;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerLandingState : PlayerBaseState
    {
        #region Statements

        private enum StateLanding
        {
            NoLanding,
            Landing,
            HardLanding
        }
        
        private StateLanding _stateLanding;

        public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private StateLanding CheckVelocity()
        {
            var velocity = StateMachine.Velocity.y;
            
            if (velocity <= StateMachine.MaxHardLanding) return StateLanding.HardLanding;
            if (velocity <= StateMachine.MaxLanding) return StateLanding.Landing;

            return StateLanding.NoLanding;
        }

        #endregion

        #region Events

        public override void Enter()
        {
            _stateLanding = CheckVelocity();

            var idAnim = _stateLanding switch
            {
                StateLanding.NoLanding => PlayerAnimationIds.JumpingDown,
                StateLanding.Landing => PlayerAnimationIds.Landing,
                StateLanding.HardLanding => PlayerAnimationIds.HardLanding,
                _ => PlayerAnimationIds.JumpingDown
            };
            
            StateMachine.Animator.CrossFadeInFixedTime(idAnim, .1f);
        }

        public override void Tick(float deltaTime)
        {
            if (IsAnimationInTransition()) return;
            
            if (_stateLanding == StateLanding.NoLanding)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            if (normalizedTime > 0.9f && StateMachine.IsGrounded())
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
        }

        #endregion
    }
}