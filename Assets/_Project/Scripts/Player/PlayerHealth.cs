using UnityEngine;

/// <summary>
/// Simple player health. Takes damage from plaque germs, with a short
/// invincibility window so the player is not drained every frame.
/// On reaching 0 HP it shows a message and reloads the scene to try again.
/// Attach to the Player.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 5;
    [Tooltip("Current HP (read-only at runtime).")]
    public int currentHP;

    [Header("Damage")]
    [Tooltip("Seconds of invincibility after taking a hit.")]
    public float invincibilityTime = 1f;

    [Tooltip("Sprite that flashes while invincible (usually the Player sprite).")]
    public SpriteRenderer spriteToFlash;

    // UI listens to this to update the HP text.
    public System.Action<int, int> OnHealthChanged;

    private float invincibleTimer;
    private bool isDead;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        if (OnHealthChanged != null) OnHealthChanged(currentHP, maxHP);
    }

    private void Update()
    {
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;

            // Blink the sprite while invincible.
            if (spriteToFlash != null)
                spriteToFlash.enabled = (Mathf.FloorToInt(invincibleTimer * 10f) % 2 == 0);

            if (invincibleTimer <= 0f && spriteToFlash != null)
                spriteToFlash.enabled = true; // make sure it ends visible
        }
    }

    /// <summary>Called by GermEnemy on contact.</summary>
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (invincibleTimer > 0f) return; // still invincible from the last hit

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        invincibleTimer = invincibilityTime;
        SfxPlayer.PlayDamage();

        if (OnHealthChanged != null) OnHealthChanged(currentHP, maxHP);

        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        isDead = true;

        DialogueManager.Instance.ShowDialogue(new string[]
        {
            "Oh no! The plaque germs got you. Try again!"
        });

        // Freeze the player, then reload the scene for a fresh try.
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Dialogue);
        Invoke(nameof(ReloadScene), 2.5f);
    }

    private void ReloadScene()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);
    }

    /// <summary>True during the invincibility window (handy for debugging).</summary>
    public bool IsInvincible()
    {
        return invincibleTimer > 0f;
    }
}
