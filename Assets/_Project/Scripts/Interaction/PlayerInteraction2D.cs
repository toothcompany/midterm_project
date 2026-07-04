using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Lets the player interact with nearby InteractableObjects by pressing Space.
/// Attach to the Player. Nearby objects are detected by the child
/// "InteractionTrigger" (CircleCollider2D with Is Trigger on) — because that
/// trigger is attached to the Player's Rigidbody2D, its OnTriggerEnter2D /
/// OnTriggerExit2D events arrive on this script.
/// </summary>
public class PlayerInteraction2D : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("Text element that shows '[Space] Talk' etc. when near an interactable.")]
    public TMP_Text promptText;

    // All interactables currently inside the trigger circle.
    private readonly List<InteractableObject> nearby = new List<InteractableObject>();

    private void Update()
    {
        // Picked-up objects get destroyed; remove empty entries.
        nearby.RemoveAll(item => item == null);

        // Only interact while exploring (not during dialogue or puzzles).
        bool exploring = GameManager.Instance == null || GameManager.Instance.IsExploring();

        InteractableObject current = exploring ? GetClosest() : null;

        // Show or hide the prompt.
        if (promptText != null)
        {
            promptText.text = current != null ? "[Space] " + current.interactionPrompt : "";
        }

        if (current == null) return;

        // If a dialogue was closed with Space this same frame, skip this frame so
        // the same key press does not instantly start the next interaction.
        if (DialogueManager.Instance != null && DialogueManager.Instance.ClosedThisFrame()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            current.Interact();
        }
    }

    /// <summary>Returns the nearest interactable inside the trigger, or null.</summary>
    private InteractableObject GetClosest()
    {
        InteractableObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (InteractableObject item in nearby)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = item;
            }
        }
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if (interactable != null && !nearby.Contains(interactable))
        {
            nearby.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if (interactable != null)
        {
            nearby.Remove(interactable);
        }
    }
}
