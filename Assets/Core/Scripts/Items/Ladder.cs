using UnityEngine;

namespace Core.Scripts.Items
{
    public class Ladder : MonoBehaviour
    {
        #region Statements

        [Header("Ladder transform references")]
        [SerializeField] private Transform _ladderTop;
        [SerializeField] private Transform _ladderBottom;
        [SerializeField] private Transform _offsetTop;
        [SerializeField] private Transform _offsetBottom;
        
        [Space] [SerializeField] private bool _canClimbOnTop;
        
        public Transform LadderTop => _ladderTop;
        public Transform LadderBottom => _ladderBottom;
        public Transform OffsetTop => _offsetTop;
        public Transform OffsetBottom => _offsetBottom;
        public bool CanClimbOnTop => _canClimbOnTop;

        #endregion
    }
}
