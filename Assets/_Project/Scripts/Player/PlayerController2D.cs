using UnityEngine;

/// <summary>
/// Simple top-down 2D player movement using a Rigidbody2D.
/// Attach to the Player GameObject (needs Rigidbody2D + a Collider2D).
/// Rigidbody2D settings: Body Type = Dynamic, Gravity Scale = 0, Freeze Rotation Z.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Set to false to freeze the player (used during dialogue and puzzles).
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Arrow keys / WASD via the built-in Input Manager.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Normalize so diagonal movement is not faster.
        moveInput = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        // Move with physics so we collide with walls properly.
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>Freeze or unfreeze the player. Called by GameManager.</summary>
    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
