using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 6f;                 // Speed going uphill (towards the player)
    [SerializeField] private float activationDistance = 30f;   // Starts moving when the player is this close
    [SerializeField] private float groundCheckDistance = 1.5f; // Extra raycast length below the enemy to detect the floor
    [SerializeField] private float rayStartHeight = 2f;        // The raycast starts this far above the enemy, so it never begins below the floor
    [SerializeField] private float rotationSpeed = 10f;        // How fast the enemy tilts to match the slope
    [SerializeField] private LayerMask floorLayer;             // Select the "Floor" layer in the Inspector

    Rigidbody2D rb;
    Transform player;
    bool isActive;
    float groundOffset;     // Distance from the enemy's center to the bottom of its collider
    float verticalVelocity; // Manual gravity, used only when there is no floor below
    Collider2D ownCollider;
    ContactFilter2D floorFilter;
    readonly RaycastHit2D[] rayHits = new RaycastHit2D[4];

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ownCollider = GetComponent<Collider2D>();

        floorFilter = new ContactFilter2D();
        floorFilter.SetLayerMask(floorLayer);

        // Kinematic: gravity and effectors (like the Iceberg's Surface Effector) don't move it,
        // we move it ourselves. It still collides with the dynamic Player.
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Distance from the enemy's pivot down to the bottom of its collider.
        // Uses bounds.min instead of half the height, so it also works when the collider
        // has an Offset (e.g. a small capsule placed at the feet / board).
        groundOffset = transform.position.y - ownCollider.bounds.min.y;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        if (floorLayer.value == 0)
            Debug.LogWarning($"{name}: 'Floor Layer' está en Nothing. Selecciona la capa Floor en el Inspector.");
        if (gameObject.layer == LayerMask.NameToLayer("Floor"))
            Debug.LogWarning($"{name}: el enemigo NO debe estar en la capa Floor.");
    }

    void FixedUpdate()
    {
        // Wait until the player is near, so the enemy doesn't walk away before we see it
        if (!isActive && player != null &&
            Vector2.Distance(player.position, rb.position) <= activationDistance)
        {
            isActive = true;
        }

        MoveAlongFloor(isActive ? speed : 0f);
    }

    /// <summary>
    /// Keeps the enemy glued to the floor surface and moves it along the slope
    /// to the left (towards the player). With moveSpeed = 0 it just stays still on the floor.
    /// </summary>
    private void MoveAlongFloor(float moveSpeed)
    {
        Vector2 rayOrigin = rb.position + Vector2.up * rayStartHeight;
        float rayLength = rayStartHeight + groundOffset + groundCheckDistance;
        RaycastHit2D hit = FindFloor(rayOrigin, rayLength);

        if (hit)
        {
            verticalVelocity = 0f;

            // Direction parallel to the floor, pointing left
            Vector2 tangent = new Vector2(-hit.normal.y, hit.normal.x);

            // Place the enemy on the surface, moving it only vertically (directly above the hit point).
            // Dividing by normal.y keeps the perpendicular distance to the slope equal to groundOffset.
            // (Moving it along the normal would shift X every frame and make it slide downhill.)
            float heightAboveHit = groundOffset / Mathf.Max(hit.normal.y, 0.1f);
            Vector2 surfacePosition = new Vector2(rb.position.x, hit.point.y + heightAboveHit);

            // Advance along the slope (to the left)
            rb.MovePosition(surfacePosition + tangent * (moveSpeed * Time.fixedDeltaTime));

            // Tilt the enemy to match the slope
            float targetAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime));
        }
        else
        {
            // No floor below (in the air): a kinematic body has no gravity, so we apply it manually
            verticalVelocity += Physics2D.gravity.y * Time.fixedDeltaTime;
            Vector2 velocity = new Vector2(-moveSpeed, verticalVelocity);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Casts a ray downwards and returns the first floor hit that is NOT the enemy's own collider.
    /// (If the enemy were on the Floor layer, the ray would hit itself and push it upwards.)
    /// </summary>
    private RaycastHit2D FindFloor(Vector2 origin, float length)
    {
        int count = Physics2D.Raycast(origin, Vector2.down, floorFilter, rayHits, length);
        for (int i = 0; i < count; i++)
        {
            if (rayHits[i].collider != ownCollider)
                return rayHits[i]; // Hits are sorted by distance, so this is the closest floor
        }
        return default;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null && collision.rigidbody.CompareTag("Player"))
        {
            CrashDetector crashDetector = collision.rigidbody.GetComponentInChildren<CrashDetector>();
            if (crashDetector != null)
            {
                crashDetector.Crash();
            }
        }
    }

    // Draws the floor raycast in the Scene view to help adjust rayStartHeight and groundCheckDistance
    void OnDrawGizmosSelected()
    {
        float extents = groundOffset > 0f ? groundOffset : transform.position.y - GetComponent<Collider2D>().bounds.min.y;
        Vector3 origin = transform.position + Vector3.up * rayStartHeight;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector3.down * (rayStartHeight + extents + groundCheckDistance));
    }
}
