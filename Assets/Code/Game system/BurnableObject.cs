using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    public float burnRadius = 5f; // Radius within which the fire can burn this object
    public float burnDuration = 3f; // Time it takes for the object to burn completely
    public GameObject fireEffect; // Optional: Visual effect for burning

    private bool isBurning = false;
    private float burnTimer = 0f;
    private GameObject spawnedFireEffect; // Reference to the spawned fire effect

    void Update()
    {
        // Check for nearby fire sources
        if (!isBurning)
        {
            CheckForFire();
        }
        else
        {
            // Handle burning process
            burnTimer += Time.deltaTime;
            if (burnTimer >= burnDuration)
            {
                // Destroy the spawned fire effect if it exists
                if (spawnedFireEffect != null)
                {
                    Destroy(spawnedFireEffect);
                }

                Destroy(gameObject); // Destroy the object after burning
            }
        }
    }

    void CheckForFire()
    {
        // Find all objects with the "Fire" tag within the burn radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, burnRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fire"))
            {
                StartBurning();
                break;
            }
        }
    }

    void StartBurning()
    {
        isBurning = true;
        Debug.Log(gameObject.name + " is burning!");

        // Optional: Instantiate a fire effect and store the reference
        if (fireEffect != null)
        {
            spawnedFireEffect = Instantiate(fireEffect, transform.position, Quaternion.identity);
        }
    }
}