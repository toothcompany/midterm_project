using UnityEngine;

/// <summary>
/// The lost toothbrush. Hidden until all plaque is cleaned (PlaqueManager
/// activates it). Interacting picks it up and removes it from the scene.
/// </summary>
public class ToothbrushPickup : InteractableObject
{
    public override void Interact()
    {
        InventoryManager.Instance.PickUpToothbrush();
        SfxPlayer.PlayPickup();
        DialogueManager.Instance.ShowDialogue(new string[]
        {
            "You found the lost toothbrush!"
        });

        Destroy(gameObject);
    }
}
