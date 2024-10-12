using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetection : MonoBehaviour
{
    public GameObject ShopIndicator;  // Assign the UI or GameObject you want to enable in the Inspector.
    public string shopLayerName = "Shop";  // Make sure the layer is named "Shop" in Unity.

    private void Start()
    {
        if (ShopIndicator != null)
            ShopIndicator.SetActive(false);  // Ensure the UI is disabled by default.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(shopLayerName))
        {
            if (ShopIndicator != null)
                ShopIndicator.SetActive(true);  // Enable the UI/GameObject when the player enters the shop.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(shopLayerName))
        {
            if (ShopIndicator != null)
                ShopIndicator.SetActive(false);  // Disable the UI/GameObject when the player exits the shop.
        }
    }
}
