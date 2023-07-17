using Core.Scripts.Controllers.StateMachines.Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Scripts.Controllers.RendererFeatures
{
    public class RendererFeaturesController : MonoBehaviour
    {
        #region Statements

        public ScriptableRendererFeature HintVisionFeature;
        public ScriptableRendererFeature HintAllFeature;
        public ScriptableRendererFeature HintRedFeature;
        public ScriptableRendererFeature HintBlueFeature;
        public ScriptableRendererFeature HintGreenFeature;
        
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
            
            HintVisionFeature.SetActive(_hintVisionValue);
            HintAllFeature.SetActive(_hintVisionValue);
            HintRedFeature.SetActive(_hintVisionValue);
            HintBlueFeature.SetActive(_hintVisionValue);
            HintGreenFeature.SetActive(_hintVisionValue);
        }

        #endregion
    }
}
