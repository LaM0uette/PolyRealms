using UnityEngine;

namespace Core.Scripts.StaticUtilities
{
    public class PlayerAnimationIds : MonoBehaviour
    {
        #region Locomotion

        public static readonly int LocomotionBlendTree = Animator.StringToHash("LocomotionBlendTree");
        public static readonly int LocomotionSpeed = Animator.StringToHash("LocomotionSpeed");

        #endregion
        
        #region Actions

        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int Fall = Animator.StringToHash("Fall");

        #endregion
    }
}
