﻿using System;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbLadderState : PlayerBaseState
    {
        private readonly Transform _ladder;

        public PlayerClimbLadderState(PlayerStateMachine stateMachine, Transform ladder) : base(stateMachine)
        {
            _ladder = ladder;
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            float verticalInput = StateMachine.Inputs.MoveValue.y;

            // Faire face à l'échelle
            Vector3 lookAtPosition = _ladder.position;
            lookAtPosition.y = StateMachine.transform.position.y;
            StateMachine.transform.LookAt(lookAtPosition);

            StateMachine.transform.Translate(0, verticalInput * StateMachine.LadderSpeed * deltaTime, 0);

            if (verticalInput > 0)
            {
                StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderUp);
            }
            else if (verticalInput < 0)
            {
                StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderDown);
            }
            
            // Ajout de la vérification pour descendre de l'échelle
            if (verticalInput < 0 && StateMachine.transform.position.y <= _ladder.position.y)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
                return;
            }
            
            StateMachine.Animator.speed = Math.Abs(verticalInput);
        }

        public override void TickLate(float deltaTime)
        {
            CameraRotation();
        }

        public override void Exit()
        {
        }
    }
}