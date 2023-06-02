using Core.Scripts.Controllers.StateMachines.Player;
using Core.Scripts.Items;
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
            if (_stateMachine.IsClimbing) return;
            
            if (other.TryGetComponent(out Ladder ladder))
            {
                _stateMachine.IsClimbing = true;
                _stateMachine.SwitchState(new PlayerClimbLadderState(_stateMachine, ladder));
            }
        }

        private void OnAnimatorMove()
        {
            if (!_stateMachine.UseRootMotion) return;

            _stateMachine.Animator.ApplyBuiltinRootMotion();
            transform.rotation *= _stateMachine.Animator.deltaRotation;
        }

        #endregion
    }
}