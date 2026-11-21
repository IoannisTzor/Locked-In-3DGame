using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Looks for Interactable objects in front of the camera, shows the prompt,
// and calls Interact() when the player presses E.
public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text promptLabel;   // e.g. "Press E to inspect"
    [SerializeField] private TMP_Text inspectLabel;  // description shown while inspecting

    [Header("Settings")]
    [SerializeField] private float interactRange = 2.5f;
    [SerializeField] private LayerMask interactMask = ~0;

    public Camera PlayerCamera => playerCamera;
    public bool IsBusy { get; private set; }

    private FirstPersonController controller;
    private int busyChangedFrame = -1;

    private void Awake()
    {
        controller = GetComponent<FirstPersonController>();
        SetPrompt(null);
        ShowInspectText(null);
    }

    private void Update()
    {
        // Skip while inspecting, and on the exact frame busy state changed
        // so one E press doesn't both close and reopen an item.
        if (IsBusy || Time.frameCount == busyChangedFrame)
        {
            SetPrompt(null);
            return;
        }

        Interactable target = FindInteractable();
        SetPrompt(target != null ? target.PromptText : null);

        if (target != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            target.Interact(this);
        }
    }

    // Called by things like InspectableItem to freeze/unfreeze the player.
    public void SetBusy(bool busy)
    {
        IsBusy = busy;
        busyChangedFrame = Time.frameCount;
        if (controller != null) controller.CanControl = !busy;
    }

    public void ShowInspectText(string text)
    {
        if (inspectLabel == null) return;
        bool show = !string.IsNullOrEmpty(text);
        inspectLabel.gameObject.SetActive(show);
        if (show) inspectLabel.text = text;
    }

    private Interactable FindInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponentInParent<Interactable>();
        }
        return null;
    }

    private void SetPrompt(string text)
    {
        if (promptLabel == null) return;
        bool show = !string.IsNullOrEmpty(text);
        promptLabel.gameObject.SetActive(show);
        if (show) promptLabel.text = text;
    }
}
