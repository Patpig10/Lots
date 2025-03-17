using UnityEngine;
using UnityEngine.UI;

public class FireBossHealth : MonoBehaviour
{
    public int totalHealth = 3600; // Total health of the boss (1200 per part * 3 parts)
    public Slider healthBar; // Reference to the health bar slider
    public GameObject emblem; // Object to spawn when boss is destroyed
    public GameObject spawnpoint; // Reference to the spawn point for the emblem
    private int currentHealth; // Current health of the boss
    private bool isDefeated = false; // Track if the boss is defeated
    public GameObject hitVFX;
    public GameObject blockBurstEffect;
    public GameObject hitSFX;
    private void Start()
    {
        currentHealth = totalHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = totalHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {

        hitSFX.GetComponent<AudioSource>().Play();
        if (hitVFX != null)
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
        if (isDefeated) return; // Ignore damage if the boss is already defeated

        currentHealth -= damage;

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Check if the boss is defeated
        if (currentHealth <= 0)
        {
            DefeatBoss();
        }
    }

    private void DefeatBoss()
    {
        if (blockBurstEffect != null)
        {
            Instantiate(blockBurstEffect, transform.position, Quaternion.identity);
        }
        Debug.Log("DefeatBoss called!");

        isDefeated = true;

        // Check if spawnpoint is assigned
        if (spawnpoint == null)
        {
            Debug.LogError("Spawnpoint is not assigned!");
            return;
        }

        // Check if emblem is assigned
        if (emblem == null)
        {
            Debug.LogError("Emblem prefab is not assigned!");
            return;
        }

        // Get the spawnpoint's position
        Vector3 spawnPosition = spawnpoint.transform.position;

        // Set the Y position to 1.737
        spawnPosition.y = 1.737f;

        // Log the modified spawn position
        Debug.Log("Modified Spawn Position: " + spawnPosition);

        // Spawn the emblem at the modified position
        GameObject spawnedEmblem = Instantiate(emblem, spawnPosition, Quaternion.identity);
        if (spawnedEmblem != null)
        {
            spawnedEmblem.transform.position = new Vector3(spawnedEmblem.transform.position.x, 1.737f, spawnedEmblem.transform.position.z);
            Debug.Log("Emblem spawned at: " + spawnedEmblem.transform.position);
        }

        // Destroy all boss parts
        FireBossPart[] parts = FindObjectsOfType<FireBossPart>();
        foreach (FireBossPart part in parts)
        {
            Destroy(part.gameObject);
        }

        // Destroy the boss GameObject
        Destroy(gameObject);
        Debug.Log("Fire Boss Defeated!");
    }
}