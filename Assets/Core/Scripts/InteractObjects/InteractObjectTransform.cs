using System.Collections;
using UnityEngine;

namespace Core.Scripts.InteractObjects
{
    public abstract class InteractObjectTransform : InteractObject
    {
        #region Statements

        [SerializeField] protected Transform Destination;

        #endregion

        #region Functions

        protected IEnumerator MoveCoroutine(float speed)
        {
            var position = transform.position;
            var destinationPosition = Destination.position;
            var distanceToDestination = Vector3.Distance(position, destinationPosition);

            while (distanceToDestination > 0.01f)
            {
                position = Vector3.Lerp(position, destinationPosition, speed * Time.deltaTime);
                transform.position = position;
                distanceToDestination = Vector3.Distance(position, destinationPosition);
                yield return null;
            }

            transform.position = Destination.position;
        }

        #endregion
    }
}