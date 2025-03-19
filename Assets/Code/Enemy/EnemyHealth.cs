using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
#if UNITY_EDITOR
using UnityEditor; // Required for AssetDatabase
#endif

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // The enemy's maximum health
    private int currentHealth; // The enemy's current health
    public Animator animator; // Reference to the Animator component
    public GameObject Body;
    public GameObject damagepoints; // Prefab for the floating text
    public float textDuration = 1f; // How long the floating text stays visible
    public Vector3 textOffset = new Vector3(0, 2, 0); // Offset for the popup text
    public Vector3 Randoml = new Vector3(1, 0, 0);
    public GameObject targetObject; // The GameObject to destroy
    private Saving savingSystem;
    public GameObject potionPrefab; // Reference to the potion prefab
    public float potionSpawnChance = 0.2f; // Chance to spawn a potion (50% by default)
    public float coinSpawnChance = 0.5f; // Chance to spawn a potion (50% by default)
    public GameObject coin;

    // References to VFX prefabs (loaded from Assets/Shader folder)
    public GameObject hitVFX;
    public GameObject blockBurstEffect;
    public GameObject hitSFX;
    void Start()
    {
        savingSystem = FindObjectOfType<Saving>();

        // Initialize the enemy's health to the maximum value at the start
        currentHealth = maxHealth;

        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();

        // Load VFX prefabs from the Assets/Shader folder (Editor only)

      //  hitVFX = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Shaders/HitVFX.prefab");
       // blockBurstEffect = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Shaders/BlockBurstEffect.prefab");


        // Check if the prefabs were loaded successfully
        if (hitVFX == null)
        {
            Debug.LogError("HitVFX prefab not found in Assets/Shader folder.");
        }
        if (blockBurstEffect == null)
        {
            Debug.LogError("BlockBurstEffect prefab not found in Assets/Shader folder.");
        }
    }

    // This function will be called when the enemy takes damage
    public void TakeDamage(int damageAmount)
    {
        damageAmount = savingSystem.weaponSavedDamage;
        // Reduce the enemy's current health by the damage amount
        currentHealth -= damageAmount;
        hitSFX.GetComponent<AudioSource>().Play();

        // Show floating damage text
        ShowFloatingText(damageAmount);

        // Play the hit VFX
        if (hitVFX != null)
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }

        // Trigger the "Hurt" animation
        //animator.SetTrigger("Hurt");

        // Check if the enemy's health has dropped to or below zero
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method when health reaches 0
        }
    }

    // Method to display floating damage text
    public void ShowFloatingText(int damageAmount)
    {
        damagepoints.transform.position += targetObject.transform.position + textOffset;
        var go = Instantiate(damagepoints, targetObject.transform.position, Quaternion.identity, transform);
        // Calculate the position with the offset

        // damagepoints.transform.position += textOffset;

        damagepoints.transform.localPosition = new Vector3(Random.Range(-Randoml.x, Randoml.x), Random.Range(-Randoml.y, Randoml.y), Random.Range(-Randoml.z, Randoml.z));

        //transform.localPosition = new Vector3(Random.Range(-Randoml.x, Randoml.x), Random.Range(-Randoml.y, Randoml.y), Random.Range(-Randoml.z, Randoml.z));

        // Set the text to display the damage amount
        TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.text = currentHealth.ToString();
            // textMesh.text = damageAmount.ToString();
        }
        else
        {
            Debug.LogError("The damagepoints prefab does not have a TextMeshPro component.");
        }

        // Destroy the floating text after a delay
        Destroy(go, textDuration);
    }

    // Method to handle what happens when the enemy dies
    private void Die()
    {
        // Spawn block burst effect if available
        if (blockBurstEffect != null)
        {
            Instantiate(blockBurstEffect, transform.position, Quaternion.identity);
        }

        Debug.LogError("deadvvvvvvv");

        // Destroy the enemy GameObject
        Destroy(Body);

        // Determine which item to spawn (potion or coin)
        float randomValue = Random.value; // Generate a single random value for both checks

        // Spawn a potion with a certain chance
        if (randomValue <= potionSpawnChance && potionPrefab != null)
        {
            // Rotate the potionPrefab by -90 degrees on the X-axis
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
            Instantiate(potionPrefab, transform.position, rotation);
        }
        // Spawn a coin only if the potion was not spawned
        else if (randomValue <= potionSpawnChance + coinSpawnChance && coin != null)
        {
            // Rotate the coin by -90 degrees on the X-axis
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
            Instantiate(coin, transform.position, rotation);
        }
    }
}