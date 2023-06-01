using System;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbLadderState : PlayerBaseState
    {
        #region Statements

        private readonly Transform _ladder;

        public PlayerClimbLadderState(PlayerStateMachine stateMachine, Transform ladder) : base(stateMachine)
        {
            _ladder = ladder;
        }

        #endregion

        #region Events

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            var MoveValueY = StateMachine.Inputs.MoveValue.y;
            var lookAtPosition = _ladder.position;
            lookAtPosition.y = StateMachine.transform.position.y;
            
            StateMachine.transform.LookAt(lookAtPosition);
            StateMachine.transform.Translate(0, MoveValueY * StateMachine.LadderSpeed * deltaTime, 0);

            switch (MoveValueY)
            {
                case > 0: StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderUp); break;
                case < 0: StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderDown); break;
            }
            
            if (MoveValueY < 0 && StateMachine.transform.position.y <= _ladder.position.y)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return;
            }
            
            StateMachine.Animator.speed = Math.Abs(MoveValueY);
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