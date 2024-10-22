using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    public int maxShieldHits = 3;  // Maximum hits the shield can take
    private int shieldHitsRemaining;
    public bool shieldActive = false;  // Flag to check if the shield is active
    public GameObject shieldPrefab;  // Reference to the shield visual effect
    private GameObject activeShield;  // Holds the spawned shield visual
    public KeyCode activateShieldKey = KeyCode.Y;  // Key to activate the shield

    private HeartSystem heartSystem;  // Reference to the HeartSystem

    private void Start()
    {
        // Get a reference to the HeartSystem component on the same GameObject
        heartSystem = GetComponent<HeartSystem>();
    }

    private void Update()
    {
        // Check if the player presses the key to activate the shield
        if (Input.GetKeyDown(activateShieldKey) && !shieldActive)
        {
            ActivateShield();
        }
    }

    // Call this method to activate the shield
    public void ActivateShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            shieldHitsRemaining = maxShieldHits;

            // Instantiate the shield visual effect
            //shieldPrefab.SetActive == true;
            shieldPrefab.SetActive(true);

            // Change all hearts to blue to show the shield is active
            if (heartSystem != null)
            {
                heartSystem.SetHeartColor(Color.blue);
            }

            Debug.Log("Shield Activated!");  // Debugging message to confirm activation
        }
    }

    // Call this method to deactivate the shield
    private void DeactivateShield()
    {
        shieldActive = false;
        shieldPrefab.SetActive(false);

        // Destroy the shield visual effect
        if (activeShield != null)
        {
            Destroy(activeShield);
        }

        // Restore heart colors back to normal
        if (heartSystem != null)
        {
            heartSystem.SetHeartColor(Color.white);
        }

        Debug.Log("Shield Deactivated!");  // Debugging message to confirm deactivation
    }

    // Method to handle damage when the shield is active
    public void TakeDamageWithShield(int damage)
    {
        if (shieldActive)
        {
            // Shield takes the damage instead of life
            shieldHitsRemaining -= 1;  // Decrease hits by 1 for each damage taken

            if (shieldHitsRemaining <= 0)
            {
                // Deactivate the shield if it has taken all hits
                DeactivateShield();
            }
        }
        else
        {
            // No shield active, damage should be passed to the heart system
            if (heartSystem != null)
            {
                heartSystem.TakeDamage(damage);
            }
        }
    }
}
