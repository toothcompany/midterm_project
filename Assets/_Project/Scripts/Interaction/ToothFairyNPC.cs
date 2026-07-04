using UnityEngine;

/// <summary>
/// The Tooth Fairy NPC.
/// The player must press Space near her to talk (no automatic dialogue).
/// The first time she plays the intro (with a warning about the germs);
/// after that she gives a context-aware reminder based on the player's progress.
/// </summary>
public class ToothFairyNPC : InteractableObject
{
    [TextArea]
    [Tooltip("Intro lines, played the first time the player talks to her.")]
    public string[] dialogueLines =
    {
        "Oh no! The toothbrush is missing!",
        "Be careful of the plaque germs.",
        "You cannot clean them safely without the toothbrush.",
        "Find the key, open the cabinet, and save the smile!"
    };

    [Tooltip("Becomes true after the intro has played. Visible for debugging.")]
    public bool hasPlayedIntro = false;

    public override void Interact()
    {
        // First time: play the full intro.
        if (!hasPlayedIntro)
        {
            hasPlayedIntro = true;
            DialogueManager.Instance.ShowDialogue(dialogueLines);
            return;
        }

        // After the intro: give a reminder that matches the player's progress.
        DialogueManager.Instance.ShowDialogue(new string[] { GetReminderLine() });
    }

    /// <summary>Pick a reminder line based on which items the player has.</summary>
    private string GetReminderLine()
    {
        InventoryManager inv = InventoryManager.Instance;

        if (inv.hasToothbrush)
            return "Go to the sink and clean the plaque from the gumline!";
        if (inv.hasToothpaste)
            return "Clean the plaque patches to reveal the toothbrush.";
        if (inv.hasKey)
            return "Use the key to open the cabinet.";
        return "Find the key first!";
    }
}
