using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShopIndicator : MonoBehaviour
{
    public GameObject shopIndicator; // The object (e.g., question mark) you want to toggle
    private bool isInShop = false; // Boolean to track whether the player is in a shop zone

    private void Start()
    {
        // Make sure the shop indicator is hidden at the start
        shopIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters a collider with the "Shop" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Shop"))
        {
            isInShop = true;
            shopIndicator.SetActive(true); // Show the shop indicator
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits a collider with the "Shop" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Shop"))
        {
            isInShop = false;
            shopIndicator.SetActive(false); // Hide the shop indicator
        }
    }
}
