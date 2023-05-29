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
        public Camera MainCamera { get; private set; }
        
        [Header("Move")]
        public float WalkSpeed = 3f;
        public float RunSpeed = 6f;
        public float JumpForce = 10f;
        public float Gravity = -9.81f;
        [HideInInspector] public Vector3 Velocity;
        
        [Header("Cinemachine")]
        [Range(0f, 100f)] public float MouseSensitivity = 30f;
        [Range(0f, 180f)] public float TopClamp = 70.0f;
        [Range(0f, -180f)] public float BottomClamp = -30.0f;
        public GameObject _cinemachineCameraTarget;

        private void Awake()
        {
            Inputs = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            MainCamera = Camera.main;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SwitchState(new PlayerIdleState(this));
        }

        #endregion

        #region Checkers

        public bool IsMoving() => !Inputs.MoveValue.Equals(Vector2.zero);

        #endregion
    }
}
