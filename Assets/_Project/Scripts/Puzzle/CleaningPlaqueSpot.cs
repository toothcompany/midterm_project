using UnityEngine;

/// <summary>
/// A single plaque spot on the tooth in the ToothCleaning mini-game.
/// Cleaned (removed) when the toothbrush touches it. Needs a trigger Collider2D.
/// </summary>
public class CleaningPlaqueSpot : MonoBehaviour
{
    private bool cleaned;

    /// <summary>Remove this spot and tell the manager. Called by the toothbrush.</summary>
    public void Clean()
    {
        if (cleaned) return;
        cleaned = true;

        SfxPlayer.PlayClean();

        if (ToothCleaningManager.Instance != null)
            ToothCleaningManager.Instance.OnSpotCleaned();

        Destroy(gameObject);
    }
}
