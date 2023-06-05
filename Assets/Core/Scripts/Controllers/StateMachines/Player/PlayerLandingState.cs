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

        #region Functions

        private int GetIdAnimation()
        {
            var velocity = StateMachine.Velocity.y;
            
            return velocity <= StateMachine.MaxHardLanding ? PlayerAnimationIds.HardLanding : PlayerAnimationIds.Landing;
        }

        private void CheckStateChange()
        {
            if (StateMachine.IsTransitioning) return;
            
            if (HasAnimationReachedStage(.95f)) 
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            var idAnim = GetIdAnimation();
            StateMachine.TransitionToAnimation(idAnim);
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            ResetVelocity();
        }

        #endregion
    }
}