using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public abstract class InteractObjectAnimator : InteractObject
    {
        #region Statements
        
        protected static readonly int InteractTrigger = Animator.StringToHash("Interact");
        protected Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        #endregion
    }
}