using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAndBridgeManager : MonoBehaviour
{
    public GameObject pushableBlockPrefab; // Prefab of the pushable block
    public Transform blockSpawnLocation;  // Location to spawn the block
    public GameObject bridge;             // Reference to the bridge GameObject

    private GameObject currentBlock;      // Reference to the current block instance
    public GameObject blocker;            // Reference to the blocker GameObject

    private GridMovement gridMovement;    // Reference to the GridMovement script
    public bool riverblock = false;


    private void Start()
    {
        // Get the GridMovement component
        gridMovement = FindObjectOfType<GridMovement>();

        // Start a coroutine to deactivate the bridge after 1 second
        StartCoroutine(DeactivateBridgeAfterDelay(0.1f));
        if(riverblock == true)
        {
            riverspawnblock();
        }
        else
        {
            SpawnBlock();
        }
       // SpawnBlock();
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
                Destroy(blocker);

                // Add GrassBlock component to all child objects of the bridge
                AddGrassBlockToBridgeChildren();

                // Update the blocks list in the GridMovement script
                if (gridMovement != null)
                {
                    gridMovement.UpdateBlocksList();
                }
            }
        }
    }

    // Method to reset the block and deactivate the bridge
    public void ResetBlockAndBridge()
    {
        // Destroy the current block if it exists
        if (currentBlock != null)
        {
            Destroy(currentBlock);
        }

        // Spawn a new block at the specified location
        SpawnBlock();

        // Deactivate the bridge
        if (bridge != null)
        {
            bridge.SetActive(false);
        }
    }

    // Helper method to spawn a new block
    private void SpawnBlock()
    {
        if (pushableBlockPrefab != null && blockSpawnLocation != null)
        {
            // Instantiate the block
            currentBlock = Instantiate(pushableBlockPrefab, blockSpawnLocation.position, blockSpawnLocation.rotation);

            // Force X rotation to 0
            Vector3 currentRotation = currentBlock.transform.rotation.eulerAngles;
            currentBlock.transform.rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
        }
    }
    public void riverspawnblock()
    {
        if (pushableBlockPrefab != null && blockSpawnLocation != null)
        {
            // Instantiate the block
            currentBlock = Instantiate(pushableBlockPrefab, blockSpawnLocation.position, blockSpawnLocation.rotation);

            // Force X rotation to 0
            Vector3 currentRotation = currentBlock.transform.rotation.eulerAngles;
            currentBlock.transform.rotation = Quaternion.Euler(0, 180, currentRotation.z);
        }

    }
    // Method to add GrassBlock component to all child objects of the bridge
    private void AddGrassBlockToBridgeChildren()
    {
        if (bridge != null)
        {
            foreach (Transform child in bridge.transform)
            {
                // Add the GrassBlock component to the child if it doesn't already have it
                if (child.GetComponent<GrassBlock>() == null)
                {
                    child.gameObject.AddComponent<GrassBlock>();
                }
            }
        }
    }
}