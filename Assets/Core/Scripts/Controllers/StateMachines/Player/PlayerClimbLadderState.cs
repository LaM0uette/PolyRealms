using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    public class PlayerClimbLadderState : PlayerBaseState
    {
        private Transform _ladder;

        public PlayerClimbLadderState(PlayerStateMachine stateMachine, Transform ladder) : base(stateMachine)
        {
            _ladder = ladder;
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void TickLate(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}