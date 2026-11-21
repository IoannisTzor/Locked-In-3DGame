using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Draws one slot per item in the player's inventory.
// The slot prefab needs a child Image named "Icon" and a child TextMeshPro text.
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private void OnEnable()
    {
        inventory.OnChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        inventory.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryItem item in inventory.Items)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);

            Transform iconTransform = slot.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image icon = iconTransform.GetComponent<Image>();
                icon.sprite = item.icon;
                icon.enabled = item.icon != null;
            }

            TMP_Text nameText = slot.GetComponentInChildren<TMP_Text>();
            if (nameText != null) nameText.text = item.displayName;
        }
    }
}
