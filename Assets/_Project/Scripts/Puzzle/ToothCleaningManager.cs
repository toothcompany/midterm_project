using TMPro;
using UnityEngine;

/// <summary>
/// Runs the ToothCleaning mini-game: counts plaque spots, updates the
/// "Plaque remaining" text, and shows the win message when all are cleaned.
/// Put this on an empty manager object and wire the references in the Inspector.
/// </summary>
public class ToothCleaningManager : MonoBehaviour
{
    public static ToothCleaningManager Instance { get; private set; }

    [Header("Plaque")]
    [Tooltip("All plaque spots in the scene (used for the starting count).")]
    public CleaningPlaqueSpot[] plaqueSpots;

    [Header("UI")]
    public TMP_Text remainingText;
    public GameObject winPanel;
    public TMP_Text winLine1;
    public TMP_Text winLine2;

    [Header("Player")]
    public ToothCleaningController toothbrush;

    [Header("State (read-only)")]
    public int remaining;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remaining = plaqueSpots != null ? plaqueSpots.Length : 0;
        UpdateUI();
        if (winPanel != null) winPanel.SetActive(false);
    }

    /// <summary>Called by a plaque spot when it is cleaned.</summary>
    public void OnSpotCleaned()
    {
        remaining--;
        UpdateUI();
        if (remaining <= 0) Win();
    }

    private void UpdateUI()
    {
        if (remainingText != null)
            remainingText.text = "Plaque remaining: " + Mathf.Max(0, remaining);
    }

    private void Win()
    {
        if (toothbrush != null) toothbrush.Freeze();

        SfxPlayer.PlayWin();
        if (winPanel != null) winPanel.SetActive(true);
        if (winLine1 != null) winLine1.text = "Perfect! You cleaned the gumline plaque!";
        if (winLine2 != null) winLine2.text = "Tooth Company has a new Smile Keeper!";

        // If a GameManager exists in this scene, mark the game won.
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Won);
    }
}
