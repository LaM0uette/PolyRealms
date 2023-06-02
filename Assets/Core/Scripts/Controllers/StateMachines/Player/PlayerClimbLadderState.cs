using System;
using Core.Scripts.Items;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbLadderState : PlayerBaseState
    {
        #region Statements

        private readonly Ladder _ladder;
        private Vector3 _startPosition, _targetPosition;
        private Quaternion _startRotation, _targetRotation;
        private bool _stopDown;
        private bool _stopUp;
        private bool _climbingUp;

        public PlayerClimbLadderState(PlayerStateMachine stateMachine, Ladder ladder) : base(stateMachine)
        {
            _ladder = ladder;
        }

        #endregion

        #region Functions

        private Vector3 GetCharPosition()
        {
            var position = _ladder.LadderTop.position + _ladder.LadderTop.forward * .3f;
            position.y = StateMachine.transform.position.y;

            if (position.y < _ladder.LadderBottom.position.y)
                position.y = _ladder.LadderBottom.position.y;

            var height = GetCapsuleHeight();
            if (position.y + height > _ladder.LadderTop.position.y)
                position.y = _ladder.LadderTop.position.y - height;

            return position;
        }

        private Quaternion GetCharRotation()
        {
            return Quaternion.LookRotation(-_ladder.LadderTop.forward);
        }

        private void CheckVerticalLimits()
        {
            if (StateMachine.transform.position.y < _ladder.LadderBottom.position.y)
                _stopDown = true;
            else if (StateMachine.transform.position.y > _ladder.LadderBottom.position.y + .15f)
                _stopDown = false;

            if (StateMachine.transform.position.y + GetCapsuleHeight() > _ladder.LadderTop.position.y)
                _stopUp = true;
            else if (StateMachine.transform.position.y + GetCapsuleHeight() < _ladder.LadderTop.position.y - .15f)
                _stopUp = false;
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            ResetCapsuleSize();
            
            var transform = StateMachine.transform;
            
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            
            _targetPosition = GetCharPosition();
            _targetRotation = GetCharRotation();
            
            _stopDown = false;
            _stopUp = false;
            _climbingUp = false;
            StateMachine.UseRootMotion = false;
            StateMachine.Animator.applyRootMotion = false;
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.ClimbingLadder, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            var MoveValueY = StateMachine.Inputs.MoveValue.y;

            if (_climbingUp)
            {
                if (StateMachine.Animator.IsInTransition(0)) return;
                
                var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
                var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);
                
                if (normalizedTime > 0.95f)
                {
                    StateMachine.transform.position += Vector3.right;
                    StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                }
                
                return;
            }
            
            var lookAtPosition = _ladder.transform.position;
            lookAtPosition.y = StateMachine.transform.position.y;
            StateMachine.transform.LookAt(lookAtPosition);
            StateMachine.transform.Translate(0, MoveValueY * StateMachine.LadderSpeed * deltaTime, 0);
            
            CheckVerticalLimits();

            if (MoveValueY < 0 && _stopDown)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return;
            }
            
            if (_stopUp && MoveValueY > 0)
            {
                if (_ladder.CanClimbOnTop)
                {
                    StateMachine.UseRootMotion = true;
                    StateMachine.Animator.applyRootMotion = true;
                    
                    //StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.ClimbingLadderTop, 0.1f);
                    StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderTop, 0, 0f);
                    _climbingUp = true;
                    
                    return;
                }
            }

            if (_climbingUp) return;
            
            AnimatorSetFloat(PlayerAnimationIds.Vertical, MoveValueY);
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            StateMachine.UseRootMotion = false;
            StateMachine.Animator.applyRootMotion = false;
            StateMachine.IsClimbing = false;
        }

        #endregion
    }
}