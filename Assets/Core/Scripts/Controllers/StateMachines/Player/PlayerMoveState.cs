using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerMoveState : PlayerBaseState
    {
        #region Statements

        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (!StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerIdleState(StateMachine));
        }

        private (float speed, float animationValue) GetSpeed()
        {
            return StateMachine.Inputs.RunValue ? (StateMachine.RunSpeed, 2f) : (StateMachine.WalkSpeed, 1f);
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity.y = Physics.gravity.y;
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
            
            var (speed, animationValue) = GetSpeed();
            
            Move(speed);
            AnimatorSetFloat(PlayerAnimationIds.LocomotionSpeed, animationValue, .1f);
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            StateMachine.Inputs.RunValue = false;
        }

        #endregion
    }
}