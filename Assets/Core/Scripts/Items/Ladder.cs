using UnityEngine;

namespace Core.Scripts.Items
{
    public class Ladder : MonoBehaviour
    {
        #region Statements

        [Header("Ladder transform references")]
        [SerializeField] private Transform _ladderTop;
        [SerializeField] private Transform _ladderBottom;
        [SerializeField] private Transform _offset;
        
        [Space] [SerializeField] private bool _canClimbOnTop;
        
        public Transform LadderTop => _ladderTop;
        public Transform LadderBottom => _ladderBottom;
        public Transform Offset => _offset;
        public bool CanClimbOnTop => _canClimbOnTop;

        #endregion

        #region Functions

        public bool IsRotated()
        {
            var rotation = transform.rotation;
            return !rotation.x.Equals(0) || !rotation.z.Equals(0);
        }

        #endregion
    }
}
