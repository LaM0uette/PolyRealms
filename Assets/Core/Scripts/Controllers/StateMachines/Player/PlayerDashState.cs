using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerDashState : PlayerBaseState
    {
        #region Statements

        public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void Dash(float deltaTime)
        {
            var distanceThisFrame = StateMachine.DashDistance / StateMachine.DashDuration * deltaTime;
            var transform = StateMachine.transform;
            var targetDirection = transform.forward;
            var dashMovement = targetDirection * distanceThisFrame;

            StateMachine.Controller.Move(dashMovement + new Vector3(0, StateMachine.Velocity.y, 0) * deltaTime);
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.TransitionToAnimation(PlayerAnimationIds.Dash);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.IsTransitioning) return;

            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            Dash(deltaTime);
            MoveRotation();
            
            if (normalizedTime > 0.8f)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}