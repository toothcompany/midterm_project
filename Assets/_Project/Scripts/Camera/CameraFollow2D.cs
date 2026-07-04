using UnityEngine;

/// <summary>
/// Smoothly follows a target (the player) on the X/Y plane.
/// Attach to the Main Camera and drag the Player into the Target field.
/// </summary>
public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The transform to follow (usually the Player).")]
    public Transform target;

    [Header("Follow Settings")]
    [Tooltip("How quickly the camera catches up. Smaller = snappier.")]
    public float smoothTime = 0.15f;

    [Tooltip("Offset from the target. Keep Z at -10 for 2D.")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
