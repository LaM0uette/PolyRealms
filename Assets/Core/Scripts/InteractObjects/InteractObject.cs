using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public abstract class InteractObject : MonoBehaviour, IInteractObject
    {
        #region Statements

        public bool IsInteracted  { get; private set; }

        #endregion
        
        #region Functions

        public abstract void Interact();
        
        protected void SetInteracted()
        {
            IsInteracted = true;
        }

        #endregion
    }
}