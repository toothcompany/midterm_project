using TMPro;
using UnityEngine;

/// <summary>
/// Simple dialogue system (singleton). Call ShowDialogue() with some lines;
/// the panel opens, the player freezes (via GameManager state), and Space
/// advances through the lines. The panel closes after the last line.
/// Put this on the GameManager object and wire the panel + text in the Inspector.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The dark panel that holds the dialogue text.")]
    public GameObject dialoguePanel;

    [Tooltip("The TextMeshPro text inside the panel.")]
    public TMP_Text dialogueText;

    [Tooltip("Small hint text ('Press Space to continue'). Child of the panel, so it hides with it.")]
    public TMP_Text continueHintText;

    private string[] lines;
    private int currentLine;
    private bool isOpen;

    // Frame guards so one key press can't open AND advance (or close AND re-open).
    private int openedFrame = -1;
    private int closedFrame = -1;

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
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen) return;

        // Ignore input on the same frame the dialogue was opened.
        if (Time.frameCount == openedFrame) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    /// <summary>Open the dialogue panel and show the first line.</summary>
    public void ShowDialogue(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;

        lines = newLines;
        currentLine = 0;
        isOpen = true;
        openedFrame = Time.frameCount;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (dialogueText != null) dialogueText.text = lines[currentLine];
        UpdateContinueHint();

        // Freeze the player while talking.
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Dialogue);
    }

    /// <summary>Show the next line, or close the dialogue after the last one.</summary>
    public void NextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            if (dialogueText != null) dialogueText.text = lines[currentLine];
            UpdateContinueHint();
        }
        else
        {
            Close();
        }
    }

    private void Close()
    {
        isOpen = false;
        closedFrame = Time.frameCount;

        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // Unfreeze the player.
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Exploring);
    }

    /// <summary>Show 'continue' normally, 'close' on the final line.</summary>
    private void UpdateContinueHint()
    {
        if (continueHintText == null) return;
        bool isLastLine = currentLine >= lines.Length - 1;
        continueHintText.text = isLastLine ? "Press Space to close" : "Press Space to continue";
    }

    /// <summary>True only on the exact frame the dialogue closed (used by PlayerInteraction2D).</summary>
    public bool ClosedThisFrame()
    {
        return Time.frameCount == closedFrame;
    }

    /// <summary>True while the dialogue panel is showing.</summary>
    public bool IsOpen()
    {
        return isOpen;
    }
}
