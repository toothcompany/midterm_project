using UnityEngine;

/// <summary>
/// Base class for everything the player can interact with (NPCs, items, cabinet...).
/// Child classes must implement Interact(), which runs when the player presses Space nearby.
/// The object needs any Collider2D so the player's InteractionTrigger can detect it.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    [Tooltip("Short action text shown in the prompt, e.g. 'Talk', 'Pick up', 'Open'.")]
    public string interactionPrompt = "Interact";

    /// <summary>Called by PlayerInteraction2D when the player presses Space near this object.</summary>
    public abstract void Interact();
}
