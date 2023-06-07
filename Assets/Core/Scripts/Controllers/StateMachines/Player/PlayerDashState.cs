using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerDashState : PlayerBaseState
    {
        #region Statements
        
        public enum DashDirection
        {
            Left,
            Right,
            Forward,
            Backward
        }

        private const float STRAFE_DISTANCE = 12f;
        private const float STRAFE_DURATION = 0.5f;
        
        private readonly DashDirection _dashDirection;

        public PlayerDashState(PlayerStateMachine stateMachine, DashDirection dashDirection) : base(stateMachine)
        {
            _dashDirection = dashDirection;
        }

        #endregion

        #region Functions

        private void Dash(float deltaTime)
        {
            var distanceThisFrame = STRAFE_DISTANCE / STRAFE_DURATION * deltaTime;
            var transform = StateMachine.MainCamera.transform;
            
            var transformRight = transform.right * distanceThisFrame;
            var transformForward = transform.forward * distanceThisFrame;

            Vector3 newTransform;
            switch (_dashDirection)
            {
                case DashDirection.Forward:
                    newTransform = transformForward;
                    SetDashBlendTree(0, 1);
                    break;
                case DashDirection.Backward:
                    newTransform = -transformForward;
                    SetDashBlendTree(0, -1);
                    break;
                case DashDirection.Left:
                    newTransform = -transformRight;
                    SetDashBlendTree(-1, 0);
                    break;
                case DashDirection.Right:
                    newTransform = transformRight;
                    SetDashBlendTree(1, 0);
                    break;
                default:
                    newTransform = transformForward;
                    break;
            }

            newTransform.y = 0;
            StateMachine.transform.position += newTransform;
        }

        private void SetDashBlendTree(float vertical, float horizontal)
        {
            AnimatorSetFloat(PlayerAnimationIds.Vertical, vertical);
            AnimatorSetFloat(PlayerAnimationIds.Horizontal, horizontal);
        }
        
        #endregion

        #region Events

        public override void Enter()
        {
            if (!StateMachine.ActiveDash)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return;
            }
            
            var rotation = StateMachine.transform.rotation.eulerAngles;
            rotation.y = StateMachine.MainCamera.transform.rotation.eulerAngles.y;
            StateMachine.transform.rotation = Quaternion.Euler(rotation);

            AnimatorSetFloat(PlayerAnimationIds.Speed, 3f);
            StateMachine.TransitionToAnimation(PlayerAnimationIds.DashBlendTree);
        }

        public override void Tick(float deltaTime)
        {
            if (StateMachine.IsTransitioning) return;

            var state = StateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            var normalizedTime = Mathf.Repeat(state.normalizedTime,1f);

            Dash(deltaTime);

            if (normalizedTime > 0.95f)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
            AnimatorSetFloat(PlayerAnimationIds.Speed, 0);
        }

        #endregion
    }
}