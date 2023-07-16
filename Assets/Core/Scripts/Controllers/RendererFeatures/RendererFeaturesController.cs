using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Scripts.Controllers.RendererFeatures
{
    public class RendererFeaturesController : MonoBehaviour
    {
        public UniversalRendererData RendererData;
        
        private static ScriptableRendererFeature _hintFeature;
        private static ScriptableRendererFeature _hidingFeature;

        private void Awake()
        {
            foreach (var feature in RendererData.rendererFeatures)
            {
                if (feature.name == "HidingObjects")
                {
                    _hidingFeature = feature;
                }
                else if (feature.name == "HintObjects")
                {
                    _hintFeature = feature;
                }
            }

            if (_hidingFeature != null) _hidingFeature.SetActive(true);
            if (_hintFeature != null) _hintFeature.SetActive(true);
        }
    }
}
