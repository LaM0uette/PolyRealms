using Core.Scripts.Items;
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

        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent += OnJump;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.JumpEvent -= OnJump;
        }

        #endregion
        
        #region Functions

        private void ResetBooleen()
        {
            _stopDown = false;
            _stopUp = false;
            _climbingUp = false;

            StateMachine.Inputs.RunValue = false;
            SetRootMotion(false);
        }

        private void UpdatePlayerPosition()
        {
            var transform = StateMachine.transform;
            var ladderPosition = _ladder.OffsetBottom.position;
            ladderPosition.y = transform.position.y;

            transform.position = ladderPosition;
            transform.rotation = _ladder.transform.rotation;
        }
        
        private void UpdateOffsetPlayerPosition()
        {
            var ladderPosition = _ladder.OffsetTop.position;
            StateMachine.transform.position = ladderPosition;
        }

        private bool CheckClimbingUp()
        {
            if (!_ladder.CanClimbOnTop) return false;
            if (!_climbingUp) return false;
            if (IsAnimationInTransition()) return false;
                
            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);
                
            if (normalizedTime > 0.95f)
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                
            return true;
        }

        private void Move(float moveValueY, float deltaTime)
        {
            StateMachine.transform.Translate(0, moveValueY * StateMachine.LadderSpeed * deltaTime, 0);
            AnimatorSetFloat(PlayerAnimationIds.Vertical, moveValueY);
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

        private void CheckLadderLimits(float moveValueY)
        {
            if (moveValueY < 0 && _stopDown)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return;
            }

            if (!_stopUp || !(moveValueY > 0)) return;
            if (!_ladder.CanClimbOnTop) return;
            
            SetRootMotion(true);
                    
            StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderTop, 0, 0f);
            _climbingUp = true;
                    
            return;
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
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
            CheckLadderLimits(MoveValueY);
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
            ResetBooleen();
            
            StateMachine.IsClimbing = false;
        }
        
        private void OnJump()
        {
            
            var transform = StateMachine.transform;
            var ladderPosition = _ladder.OffsetBottom.position;
            ladderPosition.y = transform.position.y;
            
            transform.position = ladderPosition;
            
            StateMachine.SwitchState(new PlayerJumpState(StateMachine));
        }

        #endregion
    }
}