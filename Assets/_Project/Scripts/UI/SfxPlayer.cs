using UnityEngine;

/// <summary>
/// Central sound-effect player (one per scene). Gameplay scripts call the
/// static helpers (e.g. SfxPlayer.PlayPickup()), which are safe to call even
/// if no SfxPlayer exists in the scene.
/// Put this on the GameManager object (Bathroom) or the manager object
/// (ToothCleaning) and assign the clips in the Inspector.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [Header("Clips")]
    public AudioClip pickupClip;   // key / toothpaste / toothbrush picked up
    public AudioClip damageClip;   // player hit by a germ
    public AudioClip cleanClip;    // plaque spot cleaned
    public AudioClip squeezeClip;  // toothpaste squeeze
    public AudioClip winClip;      // mini-game won

    [Header("Volume")]
    [Range(0f, 1f)] public float volume = 0.8f;

    private AudioSource source;

    private void Awake()
    {
        Instance = this; // one per scene; the scene reload replaces it
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && source != null) source.PlayOneShot(clip, volume);
    }

    // ----- Static helpers (null-safe) -----
    public static void PlayPickup()  { if (Instance != null) Instance.PlayClip(Instance.pickupClip); }
    public static void PlayDamage()  { if (Instance != null) Instance.PlayClip(Instance.damageClip); }
    public static void PlayClean()   { if (Instance != null) Instance.PlayClip(Instance.cleanClip); }
    public static void PlaySqueezeSfx() { if (Instance != null) Instance.PlayClip(Instance.squeezeClip); }
    public static void PlayWin()     { if (Instance != null) Instance.PlayClip(Instance.winClip); }
}
