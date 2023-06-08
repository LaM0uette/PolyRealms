using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

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
        public float LadderSpeed = 2f;
        public float JumpForce = 6f;
        public float DashDistance = 8f;
        public float DashDuration = 0.4f;
        [HideInInspector] public Vector3 Velocity;
        [HideInInspector] public bool IsClimbing;
        
        [Header("Parameters")]
        public float Landing = -1f;
        public float MaxLanding = -10f;
        public float MaxHardLanding = -12f;
        public float Gravity = -16f;
        [Space] 
        public float CrouchSpeedModifier = .6f;
        public float AirSpeedModifier = .8f;
        [Space] 
        [HideInInspector] public bool IsTransitioning;
        
        [Header("Cinemachine")]
        [Range(0f, 100f)] public float MouseSensitivity = 30f;
        [Range(0f, 180f)] public float TopClamp = 70.0f;
        [Range(0f, -180f)] public float BottomClamp = -30.0f;
        public GameObject _cinemachineCameraTarget;
        public CinemachineVirtualCamera[] CinemachineCameras;
        public CinemachineStateDrivenCamera CinemachineStateDrivenCamera;
        public float MinZoom = 1f;
        public float MaxZoom = 8f;
        public float ZoomForce = 10f;
        [HideInInspector] public bool UseRootMotion;
        
        [Header("Player")]
        [HideInInspector] public float InitialCapsuleHeight;
        public float JumpCapsuleHeight = 1f;
        public float CrouchCapsuleHeight = 1f;
        public float RollCapsuleHeight = .6f;
        public float SlideCapsuleHeight = .4f;
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
            
            Inputs.SwitchCameraEvent += () => { CinemachineStateDrivenCamera.enabled = !CinemachineStateDrivenCamera.enabled; };
            
            SwitchState(new PlayerMoveState(this));
        }

        #endregion

        #region Checkers

        public bool IsMoving() => !Inputs.MoveValue.Equals(Vector2.zero);
        
        public bool IsGrounded() => Controller.isGrounded;

        #endregion

        #region Animations

        public void TransitionToAnimation(int animationId, float transitionDuration = .1f)
        {
            Animator.CrossFadeInFixedTime(animationId, transitionDuration);
            IsTransitioning = true;
            StartCoroutine(EndTransitionAfterDelay(transitionDuration));
        }

        private IEnumerator EndTransitionAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            IsTransitioning = false;
        }

        #endregion
    }
}
