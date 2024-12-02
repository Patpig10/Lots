using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSegment : MonoBehaviour
{
    public int segmentHealth = 3;    // Health for each segment, except the head
    public bool isHead = false;      // Flag to check if this is the head segment
    [SerializeField] private BossHealth bossHealth; // Assign in Inspector
    public bool hit = false;


    private void Start()
    {
        // Find and reference the BossHealth script on the boss object
        GameObject bossObject = GameObject.Find("Grasslandbosshead"); // Ensure this name matches your boss GameObject
        if (bossObject != null)
        {
            bossHealth = bossObject.GetComponent<BossHealth>();
        }

        if (bossHealth == null)
        {
            Debug.LogError("BossHealth component not found on Boss object!");
        }

        if (bossHealth == null)
        {
            Debug.LogError("BossHealth reference is not assigned in the Inspector!");
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} segment taking damage: {damage}");

        // Check if bossHealth is assigned before using it
        if (bossHealth == null)
        {
            Debug.LogError("BossHealth reference is null when taking damage!");
            hit = true;
            hit = false;
            return; // Exit early to prevent further errors
        }

        segmentHealth -= damage; // Decrease the segment’s own health
        Debug.Log($"{gameObject.name} segment hit, remaining health: {segmentHealth}");

        // Apply damage to the main boss health
        if (isHead)
        {
           bossHealth.ApplyDamage(damage * 2); // Double damage for head
        }
        else
        {
            bossHealth.ApplyDamage(damage); // Standard damage for other segments
        }

        // Destroy segment if its health reaches zero
        if (segmentHealth <= 0 && !isHead)
        {
            Destroy(gameObject);
            Debug.Log($"{gameObject.name} segment destroyed");
        }
    }
}

