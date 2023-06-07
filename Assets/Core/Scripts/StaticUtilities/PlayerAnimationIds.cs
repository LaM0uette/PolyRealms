using UnityEngine;

namespace Core.Scripts.StaticUtilities
{
    public class PlayerAnimationIds : MonoBehaviour
    {
        #region Parameters

        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");

        #endregion
        
        #region BlendTree

        public static readonly int MoveBlendTree = Animator.StringToHash("MoveBlendTree");
        public static readonly int CrouchBlendTree = Animator.StringToHash("CrouchBlendTree");

        #endregion
        
        #region Actions

        public static readonly int Jump = Animator.StringToHash("Actions.Jump");
        public static readonly int Roll = Animator.StringToHash("Actions.Roll");
        public static readonly int Slide = Animator.StringToHash("Actions.Slide");
        public static readonly int Dash = Animator.StringToHash("Actions.Dash");

        #endregion

        #region Fall

        public static readonly int Fall = Animator.StringToHash("Fall.Fall");
        public static readonly int Landing = Animator.StringToHash("Fall.Landing");
        public static readonly int HardLanding = Animator.StringToHash("Fall.HardLanding");

        #endregion

        #region Climb

        public static readonly int ClimbingLadder = Animator.StringToHash("Climb.ClimbingLadder");
        public static readonly int ClimbingLadderTop = Animator.StringToHash("Climb.ClimbingLadderTop");

        #endregion
    }
}
