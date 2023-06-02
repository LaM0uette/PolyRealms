﻿using Core.Scripts.Items;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbLadderState : PlayerBaseState
    {
        #region Statements

        private readonly Ladder _ladder;
        private bool _stopDown;
        private bool _stopUp;
        private bool _climbingUp;

        public PlayerClimbLadderState(PlayerStateMachine stateMachine, Ladder ladder) : base(stateMachine)
        {
            _ladder = ladder;
        }

        #endregion

        #region Functions

        private void ResetBooleen()
        {
            _stopDown = false;
            _stopUp = false;
            _climbingUp = false;
            
            StateMachine.UseRootMotion = false;
            StateMachine.Animator.applyRootMotion = false;
        }

        private void UpdatePlayerPosition()
        {
            var transform = StateMachine.transform;
            var ladderPosition = _ladder.Offset.position;
            ladderPosition.y = transform.position.y;
            
            transform.position = ladderPosition;
            transform.rotation = _ladder.transform.rotation;
        }
        
        private void UpdateOffsetPlayerPosition()
        {
            var transform = StateMachine.transform.position;
            var ladderPosition = _ladder.LadderTop.position;
            var posY = ladderPosition.y - GetCapsuleHeight() - .15f;

            StateMachine.transform.position = new Vector3(transform.x, posY, transform.z);
        }
        
        private bool CheckClimbingUp()
        {
            if (!_ladder.CanClimbOnTop) return false;
            if (!_climbingUp) return false;
            if (StateMachine.Animator.IsInTransition(0)) return false;
                
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);
                
            if (normalizedTime > 0.95f)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                
            return true;
        }

        private void Move(float moveValueY, float deltaTime)
        {
            StateMachine.transform.Translate(0, moveValueY * StateMachine.LadderSpeed * deltaTime, 0);
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

        private bool CheckLadderLimits(float moveValueY)
        {
            if (moveValueY < 0 && _stopDown)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return true;
            }

            if (!_stopUp || !(moveValueY > 0)) return false;
            if (!_ladder.CanClimbOnTop) return false;
            
            StateMachine.UseRootMotion = true;
            StateMachine.Animator.applyRootMotion = true;
                    
            StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderTop, 0, 0f);
            _climbingUp = true;
                    
            return true;
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            ResetBooleen();
            UpdatePlayerPosition();
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.ClimbingLadder, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            var MoveValueY = StateMachine.Inputs.MoveValue.y;

            if (MoveValueY <= 0 && _stopUp && !_climbingUp) UpdateOffsetPlayerPosition();
            if (CheckClimbingUp()) return;
            
            Move(MoveValueY, deltaTime);
            
            CheckVerticalLimits();
            if (CheckLadderLimits(MoveValueY)) return;

            AnimatorSetFloat(PlayerAnimationIds.Vertical, MoveValueY);
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            ResetBooleen();
            
            StateMachine.IsClimbing = false;
        }

        #endregion
    }
}