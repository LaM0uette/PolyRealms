using UnityEngine;

namespace Core.Scripts.StaticUtilities
{
    public class PlayerAnimationIds : MonoBehaviour
    {
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        
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
        
        public static readonly int ClimbingLadder = Animator.StringToHash("Actions.ClimbingLadder");
        public static readonly int ClimbingLadderTop = Animator.StringToHash("Actions.ClimbingLadderTop");

        #endregion
    }
}
