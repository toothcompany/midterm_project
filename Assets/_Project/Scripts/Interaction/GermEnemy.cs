using UnityEngine;

/// <summary>
/// Makes a plaque patch wander slowly and damage the player on contact.
/// Added alongside PlaquePatch, so the same object still gets cleaned with Space.
/// Keep the speed low so it is not frustrating. Needs a trigger Collider2D.
/// </summary>
public class GermEnemy : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the germ drifts (units per second). Keep it slow.")]
    public float moveSpeed = 1.1f;

    [Tooltip("How often it picks a new random direction (seconds).")]
    public float changeDirectionInterval = 2f;

    [Header("Bounds (keeps germs inside the bathroom)")]
    public Vector2 minBounds = new Vector2(-6f, -4f);
    public Vector2 maxBounds = new Vector2(6f, 4f);

    [Header("Damage")]
    [Tooltip("HP removed per hit.")]
    public int damage = 1;

    private Vector2 direction;
    private float timer;

    private void Start()
    {
        PickNewDirection();
    }

    private void Update()
    {
        // Change direction now and then.
        timer += Time.deltaTime;
        if (timer >= changeDirectionInterval)
        {
            PickNewDirection();
            timer = 0f;
        }

        // Move, and bounce off the room bounds.
        Vector3 pos = transform.position + (Vector3)(direction * moveSpeed * Time.deltaTime);
        if (pos.x < minBounds.x || pos.x > maxBounds.x) direction.x = -direction.x;
        if (pos.y < minBounds.y || pos.y > maxBounds.y) direction.y = -direction.y;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }

    private void PickNewDirection()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Do not damage during dialogue / puzzle / win — only while exploring.
        if (GameManager.Instance != null && !GameManager.Instance.IsExploring()) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);
    }
}
