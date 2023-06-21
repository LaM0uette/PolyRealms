using Core.Scripts.StaticUtilities;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        #region Statements

        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion
        
        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsTransitioning) return;
            if (!HasAnimationReachedStage(.95f)) return;
            
            if (StateMachine.Inputs.AttackValue) return;

            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            SetRootMotion(true);
            StateMachine.TransitionToAnimation(PlayerAnimationIds.Slide, .2f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();

            MoveRotation(.1f);
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
            SetRootMotion(false);
        }

        #endregion
    }
}