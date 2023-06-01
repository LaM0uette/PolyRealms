using Core.Scripts.Controllers.StateMachines.Player;
using Core.Scripts.StaticUtilities;
using UnityEngine;

namespace Core.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region Statements

        private PlayerStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = GetComponent<PlayerStateMachine>();
        }

        #endregion

        #region Events

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagIds.Ladder))
            {
                _stateMachine.IsClimbing = true;
                _stateMachine.SwitchState(new PlayerClimbLadderState(_stateMachine, other.transform));
            }
            else if (other.gameObject.CompareTag(TagIds.TopLadder))
            {
                if (!_stateMachine.IsClimbing) return;

                var topPosition = other.gameObject.transform.Find("LadderTopPosition");
                _stateMachine.SwitchState(new PlayerClimbTopLadderState(_stateMachine, topPosition));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(TagIds.Ladder))
            {
                _stateMachine.IsClimbing = false;
                _stateMachine.SwitchState(new PlayerMoveState(_stateMachine));
            }
        }

        #endregion
    }
}