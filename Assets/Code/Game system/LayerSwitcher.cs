using UnityEngine;

public class LayerSwitcher : MonoBehaviour
{
    public string hiddenLayerName = "Hidden"; // Name of the "Hidden" layer

    private int hiddenLayer; // Layer index for "Hidden"
    private int playerOriginalLayer; // Store the player's original layer

    private void Start()
    {
        // Convert layer name to layer index
        hiddenLayer = LayerMask.NameToLayer(hiddenLayerName);

        // Check if the layer exists
        if (hiddenLayer == -1)
        {
            Debug.LogError($"Layer '{hiddenLayerName}' does not exist. Please create it in the Layer settings.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Store the player's original layer
            playerOriginalLayer = other.gameObject.layer;

            // Change the player's layer to "Hidden"
            other.gameObject.layer = hiddenLayer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Change the player's layer back to their original layer
            other.gameObject.layer = playerOriginalLayer;
        }
    }
}