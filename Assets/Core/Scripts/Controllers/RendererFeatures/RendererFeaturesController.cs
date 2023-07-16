using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Scripts.Controllers.RendererFeatures
{
    public class RendererFeaturesController : MonoBehaviour
    {
        public UniversalRendererData RendererData;
        
        private static ScriptableRendererFeature _normalFeature;

        private void Awake()
        {
            foreach (var feature in RendererData.rendererFeatures)
            {
                if (feature.name == "RenderObjects")
                {
                    _normalFeature = feature;
                }
            }

            if (_normalFeature != null) _normalFeature.SetActive(false);
        }
    }
}
