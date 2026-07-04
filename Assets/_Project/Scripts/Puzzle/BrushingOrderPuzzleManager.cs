using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Runs the mirror brushing-order puzzle.
/// Correct order is: Wet toothbrush (0) -> Add toothpaste (1) -> Brush teeth (2) -> Rinse (3).
/// The player clicks the four buttons in order. A wrong click resets the puzzle.
/// Completing the order wins the game.
/// Put this on an empty "BrushingOrderPuzzleManager" object and wire the UI in the Inspector.
/// </summary>
public class BrushingOrderPuzzleManager : MonoBehaviour
{
    public static BrushingOrderPuzzleManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The whole puzzle panel. Starts hidden.")]
    public GameObject puzzlePanel;

    [Tooltip("Container holding the four step buttons (hidden on win).")]
    public GameObject stepButtonsGroup;

    [Tooltip("Top line: instructions, then the first win line.")]
    public TMP_Text instructionText;

    [Tooltip("Feedback line: progress / retry message, then the second win line.")]
    public TMP_Text feedbackText;

    [Tooltip("Button that closes the puzzle and returns to exploring.")]
    public Button closeButton;

    [Header("Buttons")]
    [Tooltip("The four brushing-step buttons (each has a BrushingStepButton component).")]
    public BrushingStepButton[] stepButtons;

    [Header("State (read-only)")]
    [Tooltip("The next correct step the puzzle expects (0..3).")]
    public int nextStep;

    private const int TotalSteps = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Wire the button clicks once. This works even though the panel starts
        // hidden, because this manager sits on an always-active GameObject.
        if (stepButtons != null)
        {
            foreach (BrushingStepButton stepButton in stepButtons)
            {
                if (stepButton == null) continue;
                int step = stepButton.stepIndex; // capture for the click handler
                Button button = stepButton.GetComponent<Button>();
                if (button != null) button.onClick.AddListener(delegate { SelectStep(step); });
            }
        }

        if (closeButton != null) closeButton.onClick.AddListener(ClosePuzzle);
    }

    private void Start()
    {
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
    }

    /// <summary>Open the puzzle and freeze the player. Called by MirrorPuzzle.</summary>
    public void OpenPuzzle()
    {
        nextStep = 0;

        if (stepButtonsGroup != null) stepButtonsGroup.SetActive(true);
        if (closeButton != null) closeButton.gameObject.SetActive(true);
        if (instructionText != null) instructionText.text = "Brush in the correct order!";
        if (feedbackText != null) feedbackText.text = "Step 1 of 4";
        if (puzzlePanel != null) puzzlePanel.SetActive(true);

        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Puzzle);
    }

    /// <summary>Called when a step button is clicked (or via keyboard).</summary>
    public void SelectStep(int stepIndex)
    {
        // Only respond while the puzzle is actually running.
        if (GameManager.Instance != null && GameManager.Instance.currentState != GameState.Puzzle) return;

        if (stepIndex == nextStep)
        {
            // Correct step.
            nextStep++;
            if (nextStep >= TotalSteps)
            {
                Win();
            }
            else if (feedbackText != null)
            {
                feedbackText.text = "Step " + (nextStep + 1) + " of 4";
            }
        }
        else
        {
            // Wrong step: reset and let the player try again.
            nextStep = 0;
            if (feedbackText != null) feedbackText.text = "Almost! Try the brushing order again.";
        }
    }

    private void Win()
    {
        // Hide the buttons and show the win message on the panel.
        if (stepButtonsGroup != null) stepButtonsGroup.SetActive(false);
        if (closeButton != null) closeButton.gameObject.SetActive(false);
        if (instructionText != null) instructionText.text = "Perfect! You saved the smile!";
        if (feedbackText != null) feedbackText.text = "Tooth Company has a new Smile Keeper!";

        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Won);
    }

    /// <summary>Close the puzzle without winning and return to exploring.</summary>
    public void ClosePuzzle()
    {
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Exploring);
    }
}
