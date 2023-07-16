using Core.Scripts.Controllers.StateMachines.Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Scripts.Controllers.RendererFeatures
{
    public class RendererFeaturesController : MonoBehaviour
    {
        #region Statements

        public ScriptableRendererFeature _hidingFeature;
        public ScriptableRendererFeature _hintAllFeature;
        public ScriptableRendererFeature _hintEqualFeature;
        
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
            
            _hidingFeature.SetActive(_hintVisionValue);
            _hintAllFeature.SetActive(_hintVisionValue);
            _hintEqualFeature.SetActive(_hintVisionValue);
        }

        #endregion
    }
}
