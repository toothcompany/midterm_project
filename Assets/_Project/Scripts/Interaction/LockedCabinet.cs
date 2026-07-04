using UnityEngine;

/// <summary>
/// The locked cabinet. Needs the key to open; gives toothpaste once.
/// Attach to the Cabinet object (which already has a solid BoxCollider2D).
/// </summary>
public class LockedCabinet : InteractableObject
{
    [Tooltip("Becomes true after the cabinet has been opened. Visible for debugging.")]
    public bool isOpen = false;

    [Tooltip("Tint applied to the cabinet sprite once it is open (fallback if no open sprite is set).")]
    public Color openColor = new Color(0.75f, 0.55f, 0.35f);

    [Tooltip("Sprite shown once the cabinet is open. If empty, the tint fallback is used instead.")]
    public Sprite openSprite;

    public override void Interact()
    {
        if (isOpen)
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "The cabinet is empty now."
            });
            return;
        }

        if (!InventoryManager.Instance.hasKey)
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "The cabinet is locked. I need a key."
            });
            return;
        }

        // Player has the key: open the cabinet and hand over the toothpaste.
        isOpen = true;
        InventoryManager.Instance.PickUpToothpaste();
        SfxPlayer.PlayPickup();
        DialogueManager.Instance.ShowDialogue(new string[]
        {
            "You opened the cabinet and found toothpaste!"
        });

        // Swap to the open-cabinet sprite so the change is visible.
        // Falls back to a simple tint if no open sprite is assigned.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (openSprite != null) sr.sprite = openSprite;
            else sr.color = openColor;
        }
    }
}
