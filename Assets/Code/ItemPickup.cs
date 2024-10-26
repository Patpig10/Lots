using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public string itemName; // Set this to the name of the item in the Inspector
    private Bag playerBag;

    private void Start()
    {
        playerBag = FindObjectOfType<Bag>(); // Finds the Bag component on the player
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBag.AddItem(itemName); // Add the item to the bag
            PopupManager.Instance.ShowPopup($"{itemName} added to bag!"); // Show popup
            Destroy(gameObject); // Destroy the item GameObject
        }
    }
}
