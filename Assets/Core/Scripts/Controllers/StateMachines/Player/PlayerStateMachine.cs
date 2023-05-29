using System;
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
        public Camera MainCamera { get; private set; }
        
        [Header("Move")]
        public float WalkSpeed = 3f;
        public float RunSpeed = 6f;
        //public float JumpForce = 10f;
        public float Gravity = -9.81f;

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
        
        #region Functions

        public void CameraRotation()
        {
            var deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += Inputs.LookValue.x * deltaTimeMultiplier * MouseSensitivity * 10;
            _cinemachineTargetPitch += Inputs.LookValue.y * deltaTimeMultiplier * MouseSensitivity * 10;

            _cinemachineTargetYaw = _cinemachineTargetYaw.ClampAngle(float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = _cinemachineTargetPitch.ClampAngle(BottomClamp, TopClamp);

            _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }

        #endregion

        #region Events

        private void LateUpdate()
        {
            CameraRotation();
        }

        #endregion
    }
}
