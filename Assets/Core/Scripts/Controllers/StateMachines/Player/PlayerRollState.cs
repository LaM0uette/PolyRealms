using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerRollState : PlayerBaseState
    {
        public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            StateMachine.Inputs.StopAnimationEvent += StopAnimation;
        }
        
        private void UnsubscribeEvents()
        {
            StateMachine.Inputs.StopAnimationEvent -= StopAnimation;
        }

        #endregion

        #region Events

        public override void Enter()
        {
            SubscribeEvents();
            
            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Roll, .2f);
        }

        public override void Tick(float deltaTime)
        {
            var speed = (StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.WalkSpeed) * 2f;
            Move(speed);
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
            StateMachine.SwitchState(new PlayerIdleState(StateMachine));
        }

        #endregion
    }
}