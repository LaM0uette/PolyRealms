using Core.Scripts.StaticUtilities;
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

        [Header("Cinemachine")]
        [Range(0f, 100f)] public float MouseSensitivity = 30f;
        [Range(0f, 180f)] public float TopClamp = 70.0f;
        [Range(0f, -180f)] public float BottomClamp = -30.0f;
        [SerializeField] private GameObject _cinemachineCameraTarget;
        
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        
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

        private void Update()
        {
            CameraRotation();
        }

        #endregion

        #region Checkers

        public bool IsMoving() => !Inputs.MoveValue.Equals(Vector2.zero);

        #endregion
        
        #region Functions

        private void CameraRotation()
        {
            var deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += Inputs.LookValue.x * deltaTimeMultiplier * MouseSensitivity * 10;
            _cinemachineTargetPitch += Inputs.LookValue.y * deltaTimeMultiplier * MouseSensitivity * 10;

            _cinemachineTargetYaw = _cinemachineTargetYaw.ClampAngle(float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = _cinemachineTargetPitch.ClampAngle(BottomClamp, TopClamp);

            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        #endregion
    }
}
