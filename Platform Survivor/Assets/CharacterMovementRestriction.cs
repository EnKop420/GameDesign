using UnityEngine;

public class CharacterMovementRestriction : MonoBehaviour
{
    public Collider2D movementBounds;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (movementBounds != null)
        {
            Vector3 clampedPosition = transform.position;
            Bounds bounds = movementBounds.bounds;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, bounds.min.x, bounds.max.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, bounds.min.y, bounds.max.y);

            transform.position = clampedPosition;
        }
    }
}
