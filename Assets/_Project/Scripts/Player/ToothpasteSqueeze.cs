using UnityEngine;

/// <summary>
/// Shows the toothpaste tube in the player's hand once it has been picked up,
/// and plays a short "squeeze" animation (squash + tilt + paste blob) when the
/// player presses Space. PlaquePatch also calls PlaySqueeze() when a patch is
/// cleaned, so the squeeze always accompanies actual cleaning.
/// Attach to the Player and wire the tube child and the blob child in the Inspector.
/// </summary>
public class ToothpasteSqueeze : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child object showing the toothpaste tube in the player's hand.")]
    public GameObject heldTube;

    [Tooltip("Small paste blob that pops out of the tube during the squeeze (child of the tube).")]
    public GameObject pasteBlob;

    [Header("Animation")]
    [Tooltip("How long one squeeze animation lasts (seconds).")]
    public float squeezeTime = 0.35f;

    [Tooltip("How much the tube squashes at the peak of the squeeze (0..1).")]
    public float squashAmount = 0.4f;

    private float timer = -1f; // < 0 means no squeeze is playing
    private Vector3 tubeBaseScale;
    private Quaternion tubeBaseRotation;

    private void Start()
    {
        if (heldTube != null)
        {
            tubeBaseScale = heldTube.transform.localScale;
            tubeBaseRotation = heldTube.transform.localRotation;
            heldTube.SetActive(false); // hidden until the toothpaste is picked up
        }
        if (pasteBlob != null) pasteBlob.SetActive(false);
    }

    private void Update()
    {
        // The tube is visible only while the player carries the toothpaste.
        bool hasPaste = InventoryManager.Instance != null && InventoryManager.Instance.hasToothpaste;
        if (heldTube != null && heldTube.activeSelf != hasPaste) heldTube.SetActive(hasPaste);
        if (!hasPaste) return;

        // A plain Space press while exploring also squeezes the tube (fun idle
        // action). Skipped while a dialogue is open or just closed, so the same
        // key press does not close a dialogue AND squeeze.
        bool exploring = GameManager.Instance == null || GameManager.Instance.IsExploring();
        bool dialogueBusy = DialogueManager.Instance != null &&
            (DialogueManager.Instance.IsOpen() || DialogueManager.Instance.ClosedThisFrame());
        if (exploring && !dialogueBusy && Input.GetKeyDown(KeyCode.Space)) PlaySqueeze();

        // Advance the running animation.
        if (timer >= 0f)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / squeezeTime);

            // Squash down and back using a sine curve, with a little tilt.
            float squash = Mathf.Sin(t * Mathf.PI) * squashAmount;
            if (heldTube != null)
            {
                heldTube.transform.localScale = new Vector3(
                    tubeBaseScale.x * (1f + squash * 0.5f), // bulge sideways
                    tubeBaseScale.y * (1f - squash),        // squash vertically
                    tubeBaseScale.z);
                heldTube.transform.localRotation =
                    tubeBaseRotation * Quaternion.Euler(0f, 0f, -20f * squash);
            }

            // Blob is visible around the middle of the squeeze.
            if (pasteBlob != null) pasteBlob.SetActive(t > 0.2f && t < 0.85f);

            // Finished: restore the tube exactly and stop.
            if (t >= 1f)
            {
                timer = -1f;
                if (heldTube != null)
                {
                    heldTube.transform.localScale = tubeBaseScale;
                    heldTube.transform.localRotation = tubeBaseRotation;
                }
                if (pasteBlob != null) pasteBlob.SetActive(false);
            }
        }
    }

    /// <summary>Start the squeeze animation (called by PlaquePatch on a successful clean).</summary>
    public void PlaySqueeze()
    {
        timer = 0f;
        SfxPlayer.PlaySqueezeSfx();
    }
}
