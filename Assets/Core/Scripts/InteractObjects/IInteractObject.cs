namespace Core.Scripts.InteractObjects
{
    public interface IInteractObject
    {
        public bool IsInteracted { get; }
        public void Interact();
    }
}