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

        #endregion

        #region Events

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            CheckStateChange();
        }

        public override void Exit()
        {
        }

        #endregion
    }
}