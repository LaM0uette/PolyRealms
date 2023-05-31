using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbTopLadderState : PlayerBaseState
    {
        private Transform _ladder;
        private AnimationCurve _climbCurve;
        private float _animationTime;

        public PlayerClimbTopLadderState(PlayerStateMachine stateMachine, Transform ladder, AnimationCurve climbCurve) : base(stateMachine)
        {
            _ladder = ladder;
            _climbCurve = climbCurve;
        }

        public override void Enter()
        {
            // Lance l'animation de monter sur le rebord
            StateMachine.Animator.Play(PlayerAnimationIds.ClimbingLadderTop);
            _animationTime = 0f;
        }

        public override void Tick(float deltaTime)
        {
            // Mise à jour du temps de l'animation
            _animationTime += deltaTime;

            // Calcul du déplacement basé sur la courbe
            float climbMovement = _climbCurve.Evaluate(_animationTime);

            // Applique le déplacement au personnage
            StateMachine.transform.Translate(0, climbMovement * deltaTime, 0);

            // Vérifie si l'animation de montée sur le rebord est terminée
            if(StateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Actions.ClimbingLadderTop") && 
               StateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
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