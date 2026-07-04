using UnityEngine;

/// <summary>
/// Very simple inventory: just true/false flags for the three story items.
/// No item lists or slots needed for this game.
/// Put this on the same "GameManager" GameObject (or its own empty object).
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Item Flags (read-only at runtime)")]
    public bool hasKey = false;
    public bool hasToothpaste = false;
    public bool hasToothbrush = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ----- Pick-up methods (called by interactable objects) -----

    public void PickUpKey()
    {
        hasKey = true;
        Debug.Log("Inventory: picked up the KEY.");
    }

    public void PickUpToothpaste()
    {
        hasToothpaste = true;
        Debug.Log("Inventory: picked up the TOOTHPASTE.");
    }

    public void PickUpToothbrush()
    {
        hasToothbrush = true;
        Debug.Log("Inventory: picked up the TOOTHBRUSH.");
    }
}
