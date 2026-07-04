using UnityEngine;

/// <summary>
/// Tracks how many plaque patches are left. When the last one is cleaned,
/// it reveals the hidden toothbrush. Put this on an empty "PlaqueManager"
/// object and assign the total count, the plaque patches, and the toothbrush.
/// </summary>
public class PlaqueManager : MonoBehaviour
{
    public static PlaqueManager Instance { get; private set; }

    [Header("References")]
    [Tooltip("The toothbrush GameObject. Starts hidden, appears when all plaque is gone.")]
    public GameObject toothbrush;

    [Tooltip("All plaque patches in the scene (used to count the total).")]
    public PlaquePatch[] plaquePatches;

    [Header("Debug (read-only)")]
    [Tooltip("How many plaque patches still need cleaning.")]
    public int remaining;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Count starts at the number of patches assigned in the Inspector.
        remaining = plaquePatches != null ? plaquePatches.Length : 0;

        // Toothbrush stays hidden until the room is clean.
        if (toothbrush != null) toothbrush.SetActive(false);
    }

    /// <summary>
    /// Called by a PlaquePatch when it is cleaned.
    /// Returns true if this was the last patch (so the caller can show the
    /// reveal message right after its own "cleaned" message).
    /// </summary>
    public bool OnPlaqueCleaned()
    {
        remaining--;

        if (remaining <= 0)
        {
            // Show the toothbrush; the PlaquePatch handles the dialogue text.
            if (toothbrush != null) toothbrush.SetActive(true);
            return true;
        }
        return false;
    }
}
