using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public abstract class InteractObjectAnimator : MonoBehaviour, IInteractObject
    {
        #region Statements

        public bool IsInteracted  { get; set; }
        
        protected static readonly int InteractTrigger = Animator.StringToHash("Interact");
        protected Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        #endregion

        #region Functions

        public abstract void Interact();

        #endregion
    }
}