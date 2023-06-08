using Core.Scripts.StaticUtilities;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerSlideState : PlayerBaseState
    {
        #region Statements

        private bool _longSlideOn;

        public PlayerSlideState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion
        
        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsTransitioning) return;
            if (!HasAnimationReachedStage(.95f)) return;

            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            SetCapsuleSize(StateMachine.SlideCapsuleHeight, StateMachine.InitialCapsuleRadius);

            SetRootMotion(true);
            StateMachine.TransitionToAnimation(PlayerAnimationIds.Slide, .2f);
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
            CameraZoom();
        }

        public override void Exit()
        {
            SetRootMotion(false);
            ResetCapsuleSize();
        }

        #endregion
    }
}