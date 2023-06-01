using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbTopLadderState : PlayerBaseState
    {
        private Transform _ladder;
        private Transform _topPosition;

        public PlayerClimbTopLadderState(PlayerStateMachine stateMachine, Transform ladder, Transform topPosition) : base(stateMachine)
        {
            _ladder = ladder;
            _topPosition = topPosition;
        }

        public override void Enter()
        {
            StateMachine.Controller.enabled = false;
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.ClimbingLadderTop, .1f);
        }

        public override void Tick(float deltaTime)
        {
            if(StateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Actions.ClimbingLadderTop") && 
               StateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
        }
        
        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            StateMachine.Controller.enabled = true;
            StateMachine.transform.position = _topPosition.position;
        }
    }
}