using UnityEngine;

namespace Core.Scripts.Controllers.StateMachines.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(InputReader))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : StateMachine
    {
        #region Statements

        public Animator Animator { get; private set; }
        public InputReader Inputs { get; private set; }
        public CharacterController Controller { get; private set; }
        public Camera MainCamera { get; private set; }
        
        [Header("Move")]
        public float WalkSpeed = 1f;
        public float NormalSpeed = 2f;
        public float RunSpeed = 5f;
        public float RollSpeed = 3f;
        public float SlideSpeed = 4.2f;
        public float LadderSpeed = 2f;
        public float JumpForce = 3f;
        [HideInInspector] public Vector3 Velocity;
        
        [Header("Cinemachine")]
        [Range(0f, 100f)] public float MouseSensitivity = 30f;
        [Range(0f, 180f)] public float TopClamp = 70.0f;
        [Range(0f, -180f)] public float BottomClamp = -30.0f;
        public GameObject _cinemachineCameraTarget;
        
        // Player
        [HideInInspector] public float InitialCapsuleHeight;
        [HideInInspector] public float InitialCapsuleRadius;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Inputs = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            MainCamera = Camera.main;
            
            InitialCapsuleHeight = Controller.height;
            InitialCapsuleRadius = Controller.radius;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SwitchState(new PlayerMoveState(this));
        }

        #endregion

        #region Checkers

        public bool IsMoving() => !Inputs.MoveValue.Equals(Vector2.zero);
        public bool IsGround() => Controller.isGrounded;

        #endregion
    }
}
