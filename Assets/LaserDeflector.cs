using UnityEngine;

public class LaserDeflector : MonoBehaviour
{
    private Mover mover;

    [SerializeField]
    private string enemyTag = "Enemy"; // Tag to identify enemies

    private void Start()
    {
        mover = GetComponent<Mover>();
        if (mover == null)
        {
            Debug.LogError($"Mover component is missing on {gameObject.name}!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log($"Laser hit wall: {other.gameObject.name}");

            // Get the collision normal
            Vector2 collisionNormal = CalculateWallNormal(other);

            // Reflect the velocity
            Vector3 currentVelocity = mover.Velocity;
            Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity.normalized, collisionNormal) * currentVelocity.magnitude;

            // Update the mover's velocity
            mover.SetVelocity(reflectedVelocity);
            Debug.Log($"Updated velocity to: {reflectedVelocity}");
        }
        else if (other.CompareTag(enemyTag))
        {
            Debug.Log($"Laser hit an enemy: {other.gameObject.name}");

            // Destroy the laser and enemy
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }

    private Vector2 CalculateWallNormal(Collider2D wallCollider)
    {
        // Determine normal based on wall name or position
        if (wallCollider.name.Contains("Top"))
            return Vector2.down; // Normal for top wall
        if (wallCollider.name.Contains("Bottom"))
            return Vector2.up; // Normal for bottom wall
        if (wallCollider.name.Contains("Left"))
            return Vector2.right; // Normal for left wall
        if (wallCollider.name.Contains("Right"))
            return Vector2.left; // Normal for right wall

        // Default fallback normal
        Debug.LogWarning("Unrecognized wall. Defaulting normal to zero.");
        return Vector2.zero;
    }
}
