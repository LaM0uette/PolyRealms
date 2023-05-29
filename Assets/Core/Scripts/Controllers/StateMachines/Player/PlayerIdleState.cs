using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        #region Statements

        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsMoving()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
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
            Move();
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}
