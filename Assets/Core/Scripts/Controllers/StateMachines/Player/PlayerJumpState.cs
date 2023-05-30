﻿using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region Functions

        private void CheckStateChange()
        {
            if (StateMachine.IsGround()) StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            else if (StateMachine.Velocity.y < 0 && !StateMachine.IsGround()) StateMachine.SwitchState(new PlayerFallState(StateMachine));
        }

        #endregion

        #region Events

        public override void Enter()
        {
            StateMachine.Velocity = new Vector3(StateMachine.Velocity.x, StateMachine.JumpForce, StateMachine.Velocity.z);

            StateMachine.Animator.CrossFadeInFixedTime(PlayerAnimationIds.Jump, .1f);
        }

        public override void Tick(float deltaTime)
        {
            ApplyGravity();
            
            var speed = (StateMachine.Inputs.RunValue ? StateMachine.RunSpeed : StateMachine.WalkSpeed) / 1.4f;
            Move(speed);
            
            CheckStateChange();
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