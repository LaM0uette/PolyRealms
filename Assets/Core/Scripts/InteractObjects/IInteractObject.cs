namespace Core.Scripts.InteractObjects
{
    public interface IInteractObject
    {
        public bool IsInteracted { get; set; }
        public void Interact();
    }
}