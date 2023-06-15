using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        #region Statements

        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion

        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.RollJumpEvent += OnRollJump;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.RollJumpEvent -= OnRollJump;
        }

        #endregion
        
        #region Functions

        private void CheckStateChange()
        {
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();

            StateMachine.TransitionToAnimation(PlayerAnimationIds.Jump);
        }

        public override void Tick(float deltaTime)
        {
            var speed = GetMoveSpeed();
            Move(speed);
            
            ApplyGravity(2.4f);

            CheckStateChange();
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
            CameraZoom();
        }

        public override void Exit()
        {
            UnsubscribeEvents();
        }

        #endregion
        
        #region InputEvents

        private void OnRollJump() => StateMachine.SwitchState(new PlayerRollJumpState(StateMachine));

        #endregion
    }
}