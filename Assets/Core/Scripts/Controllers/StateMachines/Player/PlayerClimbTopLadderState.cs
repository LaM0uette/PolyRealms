using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbTopLadderState : PlayerBaseState
    {
        #region Statements

        private readonly Transform _topPosition;

        public PlayerClimbTopLadderState(PlayerStateMachine stateMachine, Transform topPosition) : base(stateMachine)
        {
            _topPosition = topPosition;
        }

        #endregion

        #region Functions

        private void CheckStateChange()
        {
            var currentAnimatorStateInfo = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            if(currentAnimatorStateInfo.IsName("Actions.ClimbingLadderTop") && currentAnimatorStateInfo.normalizedTime >= 1)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SetControllerEnable(false);
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.ClimbingLadderTop, .1f);
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
            StateMachine.transform.position = _topPosition.position;
            SetControllerEnable(true);
        }

        #endregion
    }
}