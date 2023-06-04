using System;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerLandingState : PlayerBaseState
    {
        #region Statements

        public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.FallingToLanding, .1f);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.Animator.IsInTransition(0)) return;
            
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            if (normalizedTime > 0.9f && StateMachine.IsGrounded())
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        public override void TickLate(float deltaTime)
        {
        }

        public override void Exit()
        {
        }

        #endregion
    }
}