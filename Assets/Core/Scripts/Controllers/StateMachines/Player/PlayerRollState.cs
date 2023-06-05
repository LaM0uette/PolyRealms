using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerRollState : PlayerBaseState
    {
        public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Functions

        private void Roll(float speed)
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

        private void CheckStateChange()
        {
            if (IsAnimationInTransition()) return;
            
            if (HasAnimationReachedStage(.95f)) 
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
        }
        
        #endregion
        
        #region Events

        public override void Enter()
        {
            SetCapsuleSize(StateMachine.RollCapsuleHeight, StateMachine.InitialCapsuleRadius);
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Roll, .1f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity(4f);
            
            var speed = GetMoveSpeed() + StateMachine.RollSpeed;
            Roll(speed);
            
            MoveRotation();
            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
            ResetCapsuleSize();
        }

        #endregion
    }
}