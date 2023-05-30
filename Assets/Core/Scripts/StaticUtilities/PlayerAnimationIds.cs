using UnityEngine;

namespace Core.Scripts.StaticUtilities
{
    public class PlayerAnimationIds : MonoBehaviour
    {
        #region Locomotion

        public static readonly int MoveBlendTree = Animator.StringToHash("MoveBlendTree");
        public static readonly int CrouchBlendTree = Animator.StringToHash("CrouchBlendTree");
        
        public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        #endregion
        
        #region Actions

        public static readonly int Jump = Animator.StringToHash("Actions.Jump");
        public static readonly int Fall = Animator.StringToHash("Actions.Fall");
        public static readonly int Roll = Animator.StringToHash("Actions.Roll");
        public static readonly int Slide = Animator.StringToHash("Actions.Slide");

        #endregion
    }
}
