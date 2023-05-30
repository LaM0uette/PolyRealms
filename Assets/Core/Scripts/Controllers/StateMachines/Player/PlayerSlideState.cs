using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerSlideState : PlayerBaseState
    {
        public PlayerSlideState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.CrouchActionEvent += OnCrouchAction;
            StateMachine.Inputs.StopAnimationEvent += StopAnimation;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.CrouchActionEvent -= OnCrouchAction;
            StateMachine.Inputs.StopAnimationEvent -= StopAnimation;
        }
        
        private void OnCrouchAction()
        {
            if (!StateMachine.IsMoving()) return;
            
            StateMachine.SwitchState(new PlayerRollState(StateMachine));
        }

        #endregion

        #region Functions

        private void Slide(float speed)
        {
            var controllerVelocity = StateMachine.Controller.velocity;
            var currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0, controllerVelocity.z).magnitude;
            var targetDirection = StateMachine.transform.forward;
            
            if (currentHorizontalSpeed < speed - OFFSET || currentHorizontalSpeed > speed + OFFSET)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, speed, Time.deltaTime * 10f);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else _speed = speed;

            StateMachine.Controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0, StateMachine.Velocity.y, 0) * Time.deltaTime);
        }

        #endregion
        
        #region Events

        public override void Enter()
        {
            SubscribeEvents();

            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Slide, .2f);
        }

        public override void Tick(float deltaTime)
        {
            var speed = StateMachine.SlideSpeed + (StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.NormalSpeed);
            Slide(speed);
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
        }

        private void StopAnimation()
        {
            StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }

        #endregion
    }
}