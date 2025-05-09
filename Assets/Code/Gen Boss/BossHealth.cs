using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealth : MonoBehaviour
{

    private Saving savingSystem;

    public int mainHealth = 20; // Set the total health of the boss
    public Slider healthBar; // Reference to the health bar slider
    public GameObject brain;
    public GameObject emblem; // Object to spawn when boss is destroyed
    public GameObject blockBurstEffect;

    private void Start()
    {

        // Initialize the health bar value to the boss's maximum health
        if (healthBar != null)
        {
            healthBar.maxValue = mainHealth;
            healthBar.value = mainHealth; // Set initial value to max health
        }
    }

    public void ApplyDamage(int damage)
    {
        savingSystem = FindObjectOfType<Saving>();

        if (savingSystem != null)
        {
            // Set the weapon damage to the saved value
            damage = savingSystem.weaponSavedDamage;
        }
        mainHealth -= damage;

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = mainHealth; // Update the slider to reflect current health
        }

        // Check if the main health is depleted
        if (mainHealth <= 0)
        {
            Instantiate(emblem, transform.position, Quaternion.identity);

            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
        if (blockBurstEffect != null)
        {
            Instantiate(blockBurstEffect, transform.position, Quaternion.identity);
        }

        // Destroy all BossSegment objects
        BossSegment[] segments = FindObjectsOfType<BossSegment>();
        foreach (BossSegment segment in segments)
        {
            Destroy(segment.gameObject);
        }

        Destroy(brain);
        // Handle boss destruction (e.g., animations, loot drops, etc.)
        Destroy(gameObject);
        Debug.Log("Boss defeated!");
    }
}
