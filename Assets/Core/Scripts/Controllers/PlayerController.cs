using Core.Scripts.Controllers.StateMachines.Player;
using UnityEngine;

namespace Core.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public AnimationCurve ClimbCurve;

        private void Start()
        {
            StateMachine = GetComponent<PlayerStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ladder"))
            {
                StateMachine.SwitchState(new PlayerClimbLadderState(StateMachine, other.transform));
            }
            else if (other.gameObject.CompareTag("TopLadder"))
            {
                StateMachine.SwitchState(new PlayerClimbTopLadderState(StateMachine, other.transform, ClimbCurve));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Ladder"))
            {
                StateMachine.SwitchState(new PlayerMoveState(StateMachine));
            }
        }
    }
}
