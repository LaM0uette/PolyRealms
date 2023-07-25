using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public abstract class InteractObject : MonoBehaviour, IInteractObject
    {
        #region Statements

        protected static readonly int InteractTrigger = Animator.StringToHash("Interact");
        protected Animator _animator;
        protected bool _isInteracted;

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