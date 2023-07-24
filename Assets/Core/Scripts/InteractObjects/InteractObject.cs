using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public class InteractObject : MonoBehaviour, IInteractObject
    {
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void Interact()
        {
            _animator.enabled = true;
        }
        
        public void DisableAnimator()
        {
            _animator.enabled = false;
        }
    }
}