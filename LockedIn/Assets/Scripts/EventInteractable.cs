using UnityEngine;
using UnityEngine.Events;

// A simple "press E to make something happen" object, like moving the painting.
// It can start locked and be unlocked later by another event
// (e.g. reading the cipher note unlocks the painting).
public class EventInteractable : Interactable
{
    [Header("Lock")]
    [SerializeField] private bool startsLocked = false;
    [SerializeField] private string lockedMessage = "";

    [Header("Behavior")]
    [SerializeField] private bool onlyOnce = true;
    [SerializeField] private string message = "";

    [Header("Events")]
    public UnityEvent onInteract;

    private bool isLocked;
    private bool used;

    // Hide the prompt once it has been used up.
    public override string PromptText => (onlyOnce && used) ? "" : base.PromptText;

    private void Awake()
    {
        isLocked = startsLocked;
    }

    // Hook this up to another object's event to unlock it.
    public void Unlock()
    {
        isLocked = false;
    }

    public override void Interact(PlayerInteractor player)
    {
        if (onlyOnce && used) return;

        if (isLocked)
        {
            MessageDisplay.Show(lockedMessage);
            return;
        }

        used = true;
        MessageDisplay.Show(message);
        onInteract?.Invoke();
    }
}
