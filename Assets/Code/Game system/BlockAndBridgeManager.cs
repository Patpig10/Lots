using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAndBridgeManager : MonoBehaviour
{
    public GameObject pushableBlockPrefab; // Prefab of the pushable block
    public Transform blockSpawnLocation;  // Location to spawn the block
    public GameObject bridge;             // Reference to the bridge GameObject

    private void Start()
    {
        // Start a coroutine to deactivate the bridge after 1 second
        StartCoroutine(DeactivateBridgeAfterDelay(0.1f));
    }

    private IEnumerator DeactivateBridgeAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Deactivate the bridge
        if (bridge != null)
        {
            bridge.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Pushable" tag
        if (other.CompareTag("Pushable"))
        {
            // Destroy the block
            Destroy(other.gameObject);

            // Activate the bridge
            if (bridge != null)
            {
                bridge.SetActive(true);
            }
        }
    }

    // Method to reset the block and deactivate the bridge
    public void ResetBlockAndBridge()
    {
        // Spawn a new block at the specified location
        if (pushableBlockPrefab != null && blockSpawnLocation != null)
        {
            Instantiate(pushableBlockPrefab, blockSpawnLocation.position, blockSpawnLocation.rotation);
        }

        // Deactivate the bridge
        if (bridge != null)
        {
            bridge.SetActive(false);
        }
    }
}