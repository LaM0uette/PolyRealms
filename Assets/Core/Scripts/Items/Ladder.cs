using UnityEngine;

namespace Core.Scripts.Items
{
    public class Ladder : MonoBehaviour
    {
        [Header("Ladder transform references")]
        [SerializeField] private Transform _ladderTop;
        [SerializeField] private Transform _ladderBottom;
        
        [Space] [SerializeField] private bool _canClimbOnTop;
        
        public Transform LadderTop => _ladderTop;
        public Transform LadderBottom => _ladderBottom;
        public bool CanClimbOnTop => _canClimbOnTop;
    }
}
