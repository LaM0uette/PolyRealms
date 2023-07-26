namespace Core.Scripts.InteractObjects
{
    public class SimpleInteractObjectAnimator : InteractObjectAnimator
    {
        #region Functions

        public override void Interact()
        {
            IsInteracted = true;
            TriggerInteraction();
        }

        private void TriggerInteraction()
        {
            _animator.enabled = true;
            _animator.SetTrigger(InteractTrigger);
        }

        #endregion
    }
}