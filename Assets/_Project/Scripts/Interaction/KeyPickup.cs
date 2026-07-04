using UnityEngine;

/// <summary>
/// The bathroom key. Interacting picks it up (sets the inventory flag)
/// and removes the key from the scene.
/// </summary>
public class KeyPickup : InteractableObject
{
    public override void Interact()
    {
        InventoryManager.Instance.PickUpKey();
        SfxPlayer.PlayPickup();
        DialogueManager.Instance.ShowDialogue(new string[]
        {
            "You found the bathroom key!"
        });

        // Remove the key from the scene after picking it up.
        Destroy(gameObject);
    }
}
