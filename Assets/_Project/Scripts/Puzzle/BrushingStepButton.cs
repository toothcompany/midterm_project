using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small data component placed on each brushing-step button.
/// It simply records which step the button represents so the
/// BrushingOrderPuzzleManager can wire up the click. Set stepIndex in the
/// Inspector: 0 = Wet toothbrush, 1 = Add toothpaste, 2 = Brush teeth, 3 = Rinse.
/// </summary>
[RequireComponent(typeof(Button))]
public class BrushingStepButton : MonoBehaviour
{
    [Tooltip("0 = Wet toothbrush, 1 = Add toothpaste, 2 = Brush teeth, 3 = Rinse.")]
    public int stepIndex;
}
