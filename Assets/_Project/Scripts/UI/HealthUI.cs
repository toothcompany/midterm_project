using TMPro;
using UnityEngine;

/// <summary>
/// Shows the player's HP as simple text ("HP: 5 / 5"). Updates whenever HP
/// changes. Put this on the GameManager (or Canvas) and wire the references.
/// </summary>
public class HealthUI : MonoBehaviour
{
    [Tooltip("The player's PlayerHealth component.")]
    public PlayerHealth playerHealth;

    [Tooltip("The TextMeshPro text that shows the HP.")]
    public TMP_Text hpText;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHP;
            UpdateHP(playerHealth.currentHP, playerHealth.maxHP);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged -= UpdateHP;
    }

    private void UpdateHP(int current, int max)
    {
        if (hpText != null) hpText.text = "HP: " + current + " / " + max;
    }
}
