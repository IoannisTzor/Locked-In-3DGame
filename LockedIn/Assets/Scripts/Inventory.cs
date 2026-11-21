using System;
using System.Collections.Generic;
using UnityEngine;

// Data for one collectible item. The id is what puzzles check for (e.g. "brass_key").
[Serializable]
public class InventoryItem
{
    public string id;
    public string displayName;
    public Sprite icon;
}

// Stores the items the player is carrying. Lives on the Player object.
public class Inventory : MonoBehaviour
{
    private readonly List<InventoryItem> items = new List<InventoryItem>();

    public IReadOnlyList<InventoryItem> Items => items;

    // UI listens to this to redraw when items change.
    public event Action OnChanged;

    public void Add(InventoryItem item)
    {
        items.Add(item);
        OnChanged?.Invoke();
    }

    public bool Has(string id)
    {
        return items.Exists(i => i.id == id);
    }

    public bool Remove(string id)
    {
        int index = items.FindIndex(i => i.id == id);
        if (index < 0) return false;

        items.RemoveAt(index);
        OnChanged?.Invoke();
        return true;
    }
}
