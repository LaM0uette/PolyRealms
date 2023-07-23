using Core.Scripts.Controllers.StateMachines.Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Scripts.Controllers.RendererFeatures
{
    public class RendererFeaturesController : MonoBehaviour
    {
        #region Statements

        public ScriptableRendererFeature VisionFeature;
        public ScriptableRendererFeature VisionHintFeature;
        public ScriptableRendererFeature VisionHideFeature;
        public ScriptableRendererFeature VisionTrackFeature;
        public ScriptableRendererFeature VisionHideTrackFeature;
        
        private PlayerStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = GetComponent<PlayerStateMachine>();
        }

        #endregion

        #region Events

        private void OnEnable()
        {
            if (_stateMachine.Inputs is null) return;
            
            _stateMachine.Inputs.HintVisionEvent += OnHintVision;
        }
        
        private void OnDisable()
        {
            if (_stateMachine.Inputs is null) return;
            
            _stateMachine.Inputs.HintVisionEvent -= OnHintVision;
        }

        #endregion

        #region Functions

        private void OnHintVision()
        {
            var _hintVisionValue = _stateMachine.Inputs.HintVisionValue;
            
            VisionFeature.SetActive(_hintVisionValue);
            VisionHintFeature.SetActive(_hintVisionValue);
            VisionHideFeature.SetActive(_hintVisionValue);
            VisionTrackFeature.SetActive(_hintVisionValue);
            VisionHideTrackFeature.SetActive(_hintVisionValue);
        }

        #endregion
    }
}
