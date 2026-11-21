using UnityEngine;

// Smoothly moves/rotates an object to an "open" pose when Open() is called.
// Use it for the desk drawer, the painting over the fuse box, and the final door.
public class OpenableMover : MonoBehaviour
{
    [Tooltip("How far to move when open, in the parent's local space.")]
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0f, 0f, 0.3f);
    [Tooltip("Extra rotation when open, in degrees (e.g. 0, 90, 0 for a door).")]
    [SerializeField] private Vector3 openRotationOffset = Vector3.zero;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private AudioSource openSound;

    private Vector3 openPosition;
    private Quaternion openRotation;
    private bool isOpen;

    private void Awake()
    {
        openPosition = transform.localPosition + openPositionOffset;
        openRotation = transform.localRotation * Quaternion.Euler(openRotationOffset);
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        if (openSound != null) openSound.Play();
    }

    private void Update()
    {
        if (!isOpen) return;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition, openPosition, moveSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation, openRotation, rotateSpeed * Time.deltaTime);
    }
}
