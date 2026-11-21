using UnityEngine;
using UnityEngine.Events;

// An object the player can pick up into their inventory (key, fuse, etc.).
public class PickupItem : Interactable
{
    [SerializeField] private InventoryItem item;
    [Tooltip("Leave empty to show 'Picked up <name>'.")]
    [SerializeField] private string pickupMessage = "";

    public UnityEvent onPickedUp;

    public override void Interact(PlayerInteractor player)
    {
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("PickupItem: no Inventory found on the player.");
            return;
        }

        inventory.Add(item);

        string message = string.IsNullOrEmpty(pickupMessage)
            ? $"Picked up {item.displayName}"
            : pickupMessage;
        MessageDisplay.Show(message);

        onPickedUp?.Invoke();
        gameObject.SetActive(false);
    }
}
