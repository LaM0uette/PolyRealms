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
        public static readonly int DashBlendTree = Animator.StringToHash("DashBlendTree");

        #endregion
        
        #region Actions

        public static readonly int Jump = Animator.StringToHash("Actions.Jump");
        public static readonly int Fall = Animator.StringToHash("Actions.Fall");
        public static readonly int Roll = Animator.StringToHash("Actions.Roll");
        public static readonly int Slide = Animator.StringToHash("Actions.Slide");
        public static readonly int FallingToLanding = Animator.StringToHash("Actions.FallingToLanding");
        public static readonly int HardLanding = Animator.StringToHash("Actions.HardLanding");

        #endregion

        #region Climb

        public static readonly int ClimbingLadder = Animator.StringToHash("Climb.ClimbingLadder");
        public static readonly int ClimbingLadderTop = Animator.StringToHash("Climb.ClimbingLadderTop");

        #endregion
    }
}
