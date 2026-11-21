using UnityEngine;
using UnityEngine.Events;

// Something that needs an item to work: the locked drawer (needs the key),
// the fuse box (needs the fuse), etc. Fires onUnlocked when the right item is used.
public class ItemReceiver : Interactable
{
    [Header("Requirement")]
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem = true;

    [Header("Messages")]
    [SerializeField] private string lockedMessage = "It's locked.";
    [SerializeField] private string successMessage = "";
    [Tooltip("Prompt shown after it's unlocked. Leave empty to hide the prompt.")]
    [SerializeField] private string unlockedPrompt = "";

    [Header("Events")]
    public UnityEvent onUnlocked;

    public bool IsUnlocked { get; private set; }

    public override string PromptText => IsUnlocked ? unlockedPrompt : base.PromptText;

    public override void Interact(PlayerInteractor player)
    {
        if (IsUnlocked) return;

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null || !inventory.Has(requiredItemId))
        {
            MessageDisplay.Show(lockedMessage);
            return;
        }

        if (consumeItem) inventory.Remove(requiredItemId);

        IsUnlocked = true;
        MessageDisplay.Show(successMessage);
        onUnlocked?.Invoke();
    }
}
