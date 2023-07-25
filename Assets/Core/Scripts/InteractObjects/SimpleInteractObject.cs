namespace Core.Scripts.InteractObjects
{
    public class SimpleInteractObject : InteractObject
    {
        #region Functions

        public override void Interact()
        {
            if (_isInteracted) return;
            
            _isInteracted = true;
            
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