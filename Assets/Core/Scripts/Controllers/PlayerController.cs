using Core.Scripts.Controllers.StateMachines.Player;
using UnityEngine;

namespace Core.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStateMachine StateMachine { get; private set; }

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
