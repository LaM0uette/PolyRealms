using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    [RequireComponent(typeof(InputReader))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : StateMachine
    {
        #region Statements

        public InputReader Inputs { get; private set; }
        public CharacterController Controller { get; private set; }
        public Animator Animator { get; private set; }

        private void Awake()
        {
            Inputs = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SwitchState(new PlayerIdleState(this));
        }

        #endregion

        #region Functions

        public bool IsMoving() => !Inputs.MoveValue.Equals(Vector2.zero);

        #endregion
    }
}
