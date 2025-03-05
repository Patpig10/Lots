using UnityEngine;
using UnityEngine.UI;

public class FireBossHealth : MonoBehaviour
{
    public int totalHealth = 3600; // Total health of the boss (1200 per part * 3 parts)
    public Slider healthBar; // Reference to the health bar slider
    public GameObject emblem; // Object to spawn when boss is destroyed

    private int currentHealth; // Current health of the boss
    private bool isDefeated = false; // Track if the boss is defeated

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
        isDefeated = true;

        // Spawn the emblem
        if (emblem != null)
        {
            Instantiate(emblem, transform.position, Quaternion.identity);
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