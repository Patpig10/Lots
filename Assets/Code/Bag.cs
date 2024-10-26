using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BagItem
{
    public string itemName;
    public int count;
}

public class Bag : MonoBehaviour
{
    [SerializeField]
    private List<BagItem> items = new List<BagItem>();  // Visible in Inspector

    // Adds an item to the bag via the Inspector panel
    public void AddItem(string itemName)
    {
        BagItem item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.count++;
        }
        else
        {
            items.Add(new BagItem { itemName = itemName, count = 1 });
        }
        Debug.Log($"{itemName} added to the bag.");
    }

    // Removes an item when needed
    public void RemoveItem(string itemName)
    {
        BagItem item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.count--;
            if (item.count <= 0)
            {
                items.Remove(item);
            }
            Debug.Log($"{itemName} removed from the bag.");
        }
    }

    // Checks if the bag contains a specific item
    public bool HasItem(string itemName)
    {
        BagItem item = items.Find(i => i.itemName == itemName);
        return item != null && item.count > 0;
    }

    // Displays all items in the bag in the Inspector panel
    public List<BagItem> GetAllItems()
    {
        return items;
    }

}