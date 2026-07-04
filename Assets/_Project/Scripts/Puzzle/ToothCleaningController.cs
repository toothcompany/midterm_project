using UnityEngine;

/// <summary>
/// The player-controlled toothbrush in the ToothCleaning mini-game.
/// Moves with WASD / arrow keys inside the cleaning area and cleans plaque
/// spots it touches. Needs a Rigidbody2D and a trigger Collider2D.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ToothCleaningController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Bounds (cleaning area)")]
    public Vector2 minBounds = new Vector2(-4.5f, -3.5f);
    public Vector2 maxBounds = new Vector2(4.5f, 3.5f);

    private Rigidbody2D rb;
    private Vector2 input;
    private bool frozen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (frozen)
        {
            input = Vector2.zero;
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        input = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        Vector2 pos = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        rb.MovePosition(pos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CleaningPlaqueSpot spot = other.GetComponent<CleaningPlaqueSpot>();
        if (spot != null) spot.Clean();
    }

    /// <summary>Stop the toothbrush (called on win).</summary>
    public void Freeze()
    {
        frozen = true;
        rb.linearVelocity = Vector2.zero;
    }
}
