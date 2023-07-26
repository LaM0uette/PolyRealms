namespace Core.Scripts.InteractObjects
{
    public class SimpleInteractObjectTransform : InteractObjectTransform
    {
        #region Functions

        public override void Interact()
        {
            SetInteracted();
            StartCoroutine(MoveCoroutine(8f));
        }

        #endregion
    }
}