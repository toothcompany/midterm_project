using UnityEngine;

/// <summary>
/// The bathroom mirror / sink entry point to the tooth-cleaning mini-game.
/// With the toothbrush, interacting loads the ToothCleaning scene.
/// (The old brushing-order button puzzle is left in the project but bypassed.)
/// Attach to the Mirror object (needs a trigger Collider2D so Space can reach it).
/// </summary>
public class MirrorPuzzle : InteractableObject
{
    [Tooltip("Name of the mini-game scene to load (must be in Build Settings).")]
    public string cleaningSceneName = "ToothCleaning";

    [Tooltip("Delay before loading, so the player can read the message.")]
    public float loadDelay = 1.2f;

    public override void Interact()
    {
        // Need the toothbrush first.
        if (!InventoryManager.Instance.hasToothbrush)
        {
            DialogueManager.Instance.ShowDialogue(new string[]
            {
                "I need the toothbrush before I can clean the teeth."
            });
            return;
        }

        // Announce, freeze the player, then load the mini-game.
        DialogueManager.Instance.ShowDialogue(new string[]
        {
            "Let's clean the plaque along the gumline!"
        });
        Invoke(nameof(LoadCleaningScene), loadDelay);
    }

    private void LoadCleaningScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(cleaningSceneName);
    }
}
