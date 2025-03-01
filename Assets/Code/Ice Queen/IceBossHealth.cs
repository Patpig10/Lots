using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceBossHealth : MonoBehaviour
{
    // Boss Health Variables
    private Saving savingSystem;
    public int mainHealth = 20; // Total health of the boss
    public Slider healthBar; // Reference to the health bar slider
    public GameObject brain; // Boss's core object
    public GameObject emblem; // Object to spawn when boss is destroyed

    // Shield Variables
    public bool isShieldActive = true; // Whether the ice shield is active
    public Image healthBarFill; // Reference to the health bar fill image to change color
    public Color normalColor = Color.red; // Normal health bar color
    public Color shieldColor = Color.blue; // Health bar color when shield is active
    public GameObject shieldObject; // The shield GameObject that will melt

    // Shield Melting Variables
    public float meltRadius = 5f; // Radius within which the heat can melt the shield
    public float meltDuration = 3f; // Time it takes for the shield to melt completely
    public GameObject meltEffect; // Optional: Visual effect for melting
    private bool isMelting = false;
    private float meltTimer = 0f;
    private GameObject spawnedMeltEffect; // Reference to the spawned melt effect
    private Vector3 initialScale; // Initial scale of the shield

    // Shield Cooldown and Reset Variables
    public float shieldCooldown = 10f; // Time before the shield resets after melting
    private bool isShieldOnCooldown = false; // Whether the shield is on cooldown
    private bool isResettingShield = false; // Whether the shield is currently resetting
    private float resetTimer = 0f; // Timer for shield reset animation

    // Grace Period Variables
    public float gracePeriod = 2f; // Time after reset during which the shield cannot be melted
    private bool isGracePeriodActive = false; // Whether the grace period is active
   // public GameObject Emeblem;
    private void Start()
    {
        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = mainHealth;
            healthBar.value = mainHealth; // Set initial value to max health
        }

        // Set the health bar color based on the shield state
        UpdateHealthBarColor();

        // Store the initial scale of the shield GameObject
        if (shieldObject != null)
        {
            initialScale = shieldObject.transform.localScale;
        }
    }

    private void Update()
    {
        // Check for heat sources to melt the shield
        if (isShieldActive && !isMelting && !isGracePeriodActive && shieldObject != null)
        {
            CheckForHeat();
        }

        // Handle the melting process
        if (isMelting && shieldObject != null)
        {
            meltTimer += Time.deltaTime;
            if (meltTimer >= meltDuration)
            {
                // Shield is fully melted
                DeactivateShield();
            }
            else
            {
                // Shrink the shield over time
                float scaleFactor = 1f - (meltTimer / meltDuration);
                shieldObject.transform.localScale = initialScale * scaleFactor;
            }
        }

        // Handle the shield reset process
        if (isResettingShield && shieldObject != null)
        {
            resetTimer += Time.deltaTime;
            float resetProgress = resetTimer / meltDuration; // Use meltDuration for consistency
            if (resetProgress >= 1f)
            {
                // Shield reset complete
                ResetShieldComplete();
            }
            else
            {
                // Grow the shield back to its original size
                shieldObject.transform.localScale = initialScale * resetProgress;
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        if (isShieldActive) return; // No damage if the shield is active

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
            healthBar.value = mainHealth;
        }

        // Check if the main health is depleted
        if (mainHealth <= 0)
        {
            DestroyBoss();

            Instantiate(emblem, transform.position, Quaternion.identity);
            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
       

        Destroy(brain);
        Destroy(gameObject);
        Debug.Log("Boss defeated!");
    }

    // Check for heat sources to start melting the shield
    private void CheckForHeat()
    {
        Collider[] hitColliders = Physics.OverlapSphere(shieldObject.transform.position, meltRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fire"))
            {
                StartMelting();
                break;
            }
        }
    }

    // Start the melting process
    private void StartMelting()
    {
        isMelting = true;
        meltTimer = 0f; // Reset the melt timer
        Debug.Log("Boss shield is melting!");

        // Optional: Instantiate a melt effect
        if (meltEffect != null)
        {
            spawnedMeltEffect = Instantiate(meltEffect, shieldObject.transform.position, Quaternion.identity);
        }
    }

    // Deactivate the shield
    public void DeactivateShield()
    {
        isShieldActive = false;
        isMelting = false;
        UpdateHealthBarColor();

        // Destroy the spawned melt effect if it exists
        if (spawnedMeltEffect != null)
        {
            Destroy(spawnedMeltEffect);
        }

        Debug.Log("Boss shield deactivated!");

        // Start the shield cooldown
        StartCoroutine(ShieldCooldown());
    }

    // Shield cooldown coroutine
    private IEnumerator ShieldCooldown()
    {
        isShieldOnCooldown = true;
        Debug.Log("Shield cooldown started!");

        // Wait for the cooldown duration
        yield return new WaitForSeconds(shieldCooldown);

        // Start resetting the shield
        isResettingShield = true;
        resetTimer = 0f;
        Debug.Log("Shield resetting!");
    }

    // Complete the shield reset process
    private void ResetShieldComplete()
    {
        isResettingShield = false;
        isShieldActive = true;
        isShieldOnCooldown = false;
        shieldObject.transform.localScale = initialScale; // Ensure the shield is fully restored
        UpdateHealthBarColor();
        Debug.Log("Shield reset complete!");

        // Start the grace period
        StartCoroutine(GracePeriod());
    }

    // Grace period coroutine
    private IEnumerator GracePeriod()
    {
        isGracePeriodActive = true;
        Debug.Log("Grace period started!");

        // Wait for the grace period duration
        yield return new WaitForSeconds(gracePeriod);

        isGracePeriodActive = false;
        Debug.Log("Grace period ended!");
    }

    // Update the health bar color based on the shield state
    private void UpdateHealthBarColor()
    {
        if (healthBarFill != null)
        {
            healthBarFill.color = isShieldActive ? shieldColor : normalColor;
        }
    }
}