using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public GameObject coinPrefab;   // Reference to the coin prefab
    public GameObject keyPrefab;    // Reference to the key prefab
    public GameObject potionPrefab; // Reference to the potion prefab
    public Transform spawnPoint;    // Point where the item will be spawned

    public bool isPlayerNearby = false; // To track if the player is close to the chest

    // Booleans to control what loot items are available for this chest
    public bool canContainCoin = true;
    public bool canContainKey = true;
    public bool canContainPotion = true;
    public GameObject questionMark;

    void Update()
    {
        // Check if the player presses the 'F' key and is near the chest
        if (isPlayerNearby && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Talk")))
        {
            OpenChest(); // Call the function to open the chest
        }



    }

    private void OpenChest()
    {
        questionMark.SetActive(false);

        // Remove or hide the chest (you can also play an animation if needed)
        Destroy(gameObject); // Destroy the chest GameObject

        // Spawn a random loot item based on available loot
        SpawnRandomLoot();
    }

    private void SpawnRandomLoot()
    {
        // Create a list of potential loot items based on the enabled booleans
        List<GameObject> availableLoot = new List<GameObject>();

        if (canContainCoin)
        {
            availableLoot.Add(coinPrefab);
        }

        if (canContainKey)
        {
            availableLoot.Add(keyPrefab);
        }

        if (canContainPotion)
        {
            availableLoot.Add(potionPrefab);
        }

        // If there are no loot options, return without spawning anything
        if (availableLoot.Count == 0)
        {
            Debug.LogWarning("No loot available for this chest!");
            return;
        }

        // Randomly select one loot item from the available options
        int randomIndex = Random.Range(0, availableLoot.Count);
        GameObject selectedLoot = availableLoot[randomIndex];

        // Set up a default rotation for the loot
        Quaternion lootRotation = Quaternion.identity;

        // If the selected loot is the potion, apply the y = -90 rotation
        if (selectedLoot == potionPrefab)
        {
            lootRotation = Quaternion.Euler(-90f, 0, 0f);
        }
        if (selectedLoot == coinPrefab)
        {
            lootRotation = Quaternion.Euler(-90f, 0, 180f);
        }


        // Spawn the selected loot item at the spawn point with the appropriate rotation
        Instantiate(selectedLoot, spawnPoint.position, lootRotation);
        Debug.Log($"{selectedLoot.name} spawned!");
    }

    // Detect if the player is near the chest
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true; // Player is near the chest
        }
    }

    // Detect if the player moves away from the chest
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // Player is no longer near the chest
        }
    }
}
