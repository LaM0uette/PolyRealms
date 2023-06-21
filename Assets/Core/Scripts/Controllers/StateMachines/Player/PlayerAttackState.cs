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
            if (!HasAnimationReachedStage(.95f, 1)) return;
            
            if (StateMachine.Inputs.AttackValue) return;
            
            StateMachine.EndAttack();
            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        private void Attack()
        {
            if (!StateMachine.Inputs.AttackValue) return;
            
            StateMachine.TimerValue = 5;
            
            if (StateMachine.IsTransitioning) return;
            if (!HasAnimationReachedStage(.95f, 1)) return;
            
            StateMachine.TransitionToAnimation(PlayerAnimationIds.SwordAndShieldSlash);
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            SetRootMotion(true);
            if (!StateMachine.IsTimerRunning)  StateMachine.TransitionToAnimation(PlayerAnimationIds.DrawSword);
            StateMachine.IsTimerRunning = true;
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();

            Attack();

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