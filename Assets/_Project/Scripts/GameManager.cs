using UnityEngine;

/// <summary>
/// The overall state of the game. Other scripts check this to decide
/// what the player is allowed to do.
/// </summary>
public enum GameState
{
    Exploring,   // Player can walk around and interact.
    Dialogue,    // Dialogue box is open, player is frozen.
    Puzzle,      // Mirror brushing puzzle is open, player is frozen.
    Won          // Game finished!
}

/// <summary>
/// Central game manager (singleton). Tracks the current GameState and
/// freezes/unfreezes the player when the state changes.
/// Put this on an empty GameObject called "GameManager" in the scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Simple singleton so any script can call GameManager.Instance.
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [Tooltip("Drag the Player GameObject here.")]
    public PlayerController2D player;

    [Header("Debug (read-only)")]
    [Tooltip("Current game state, visible for debugging.")]
    public GameState currentState = GameState.Exploring;

    private void Awake()
    {
        // Standard singleton setup: keep one instance only.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>Change the game state and freeze/unfreeze the player.</summary>
    public void SetState(GameState newState)
    {
        currentState = newState;

        // The player may only move while exploring.
        if (player != null)
        {
            player.SetCanMove(newState == GameState.Exploring);
        }
    }

    /// <summary>True while the player is free to walk and interact.</summary>
    public bool IsExploring()
    {
        return currentState == GameState.Exploring;
    }
}
