using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Lets the player pick up an object, rotate it with the mouse, and put it back.
// Press E or Esc to stop inspecting. Use onInspected to trigger puzzle events
// (e.g. revealing the key inside the hollow book).
public class InspectableItem : Interactable
{
    [Header("Inspect")]
    [SerializeField] private float inspectDistance = 0.6f;
    [SerializeField] private float rotateSpeed = 0.3f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool fireEventOnce = true;

    [TextArea] [SerializeField] private string description;

    [Header("Events")]
    public UnityEvent onInspected;

    private float yaw;
    private float pitch;
    private Quaternion baseRotation;
    private PlayerInteractor player;
    private Collider[] colliders;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool inspecting;
    private bool returning;
    private int startFrame;
    private bool eventFired;
    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (inspecting || returning) return;

        player = interactor;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        yaw = 0f;
        pitch = 0f;
        baseRotation = transform.rotation;
        inspecting = true;
        startFrame = Time.frameCount;

        SetColliders(false);
        player.SetBusy(true);
        player.ShowInspectText(description);
        if (!fireEventOnce || !eventFired)
        {
            eventFired = true;
            onInspected?.Invoke();
        }
    }

    private void Update()
    {
        if (inspecting) UpdateInspecting();
        else if (returning) UpdateReturning();
    }

    private void UpdateInspecting()
    {
        Transform cam = player.PlayerCamera.transform;

        // Float in front of the camera.
        Vector3 target = cam.position + cam.forward * inspectDistance;
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);

        // Rotate with mouse movement.
        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue() * rotateSpeed;
            yaw -= delta.x;
            pitch = Mathf.Clamp(pitch + delta.y, -80f, 80f);
        }
        transform.rotation = Quaternion.AngleAxis(pitch, cam.right)
                   * Quaternion.AngleAxis(yaw, cam.up)
                   * baseRotation;

        // Ignore the same E press that started the inspection.
        if (Time.frameCount != startFrame && ExitPressed())
        {
            inspecting = false;
            returning = true;
            player.SetBusy(false);
            player.ShowInspectText(null);
        }
    }

    private void UpdateReturning()
    {
        transform.position = Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, moveSpeed * Time.deltaTime);

        if ((transform.position - originalPosition).sqrMagnitude < 0.0001f)
        {
            transform.SetPositionAndRotation(originalPosition, originalRotation);
            returning = false;
            SetColliders(true);
        }
    }

    private bool ExitPressed()
    {
        Keyboard kb = Keyboard.current;
        return kb != null && (kb.eKey.wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame);
    }

    private void SetColliders(bool enabled)
    {
        foreach (Collider c in colliders) c.enabled = enabled;
    }
}
