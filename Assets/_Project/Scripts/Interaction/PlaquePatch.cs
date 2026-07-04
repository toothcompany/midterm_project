using UnityEngine;

/// <summary>
/// A sticky plaque patch. Needs toothpaste to clean. Once cleaned it removes
/// itself and tells the PlaqueManager. Attach to each plaque object (which has
/// a trigger Collider2D so the player can stand on it and press Space).
/// </summary>
public class PlaquePatch : InteractableObject
{
    // Guards against cleaning the same patch twice.
    private bool isCleaned = false;

    public override void Interact()
    {
        if (isCleaned) return;

        // Need toothpaste first.
        if (!InventoryManager.Instance.hasToothpaste)
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "This plaque is too sticky. I need toothpaste first."
            });
            return;
        }

        // Clean it.
        isCleaned = true;

        // Play the toothpaste squeeze animation on the player.
        ToothpasteSqueeze squeeze = FindFirstObjectByType<ToothpasteSqueeze>();
        if (squeeze != null) squeeze.PlaySqueeze();

        // Tell the manager; it returns true if this was the last plaque patch.
        bool wasLast = PlaqueManager.Instance != null && PlaqueManager.Instance.OnPlaqueCleaned();

        // Show the cleaned message, plus the reveal message if this was the last one.
        if (wasLast)
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "You cleaned a plaque patch!",
                "All the plaque is gone! The toothbrush appeared!"
            });
        }
        else
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "You cleaned a plaque patch!"
            });
        }

        Destroy(gameObject);
    }
}
