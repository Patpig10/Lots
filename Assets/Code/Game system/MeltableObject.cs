using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltableObject : MonoBehaviour
{
    public float meltRadius = 5f; // Radius within which the heat can melt this object
    public float meltDuration = 3f; // Time it takes for the object to melt completely
    public GameObject meltEffect; // Optional: Visual effect for melting
    public GameObject[] itemsToActivate; // Array of inactive items to activate once melted
    public GameObject loot; // Loot to activate after melting
    public GameObject ground; // Specific GameObject to change the tag of when melted
    public GameObject meltobject; // Specific GameObject to deactivate when the block is extremely small

    private bool isMelting = false;
    private float meltTimer = 0f;
    private GameObject spawnedMeltEffect; // Reference to the spawned melt effect
    private Vector3 initialScale; // Initial scale of the object
    private bool hasDeactivatedMeltobject = false; // Track if meltobject has been deactivated

    void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;

        // Activate all items in the itemsToActivate array after 1 second
        Invoke("ActivateItems", 1f);
    }

    void Update()
    {
        // Check for nearby heat sources
        if (!isMelting)
        {
            CheckForHeat();
        }
        else
        {
            // Handle melting process
            meltTimer += Time.deltaTime;
            if (meltTimer >= meltDuration)
            {
                // Destroy the spawned melt effect if it exists
                if (spawnedMeltEffect != null)
                {
                    Destroy(spawnedMeltEffect);
                }

                // Activate the inactive items
                ActivateItems();
                loot.SetActive(true);

                // Change the tag of the specific ground GameObject to "Untagged"
                if (ground != null)
                {
                    ground.tag = "Untagged";
                }

                // Deactivate the GameObject this script is attached to
                gameObject.SetActive(false); // Set the object to inactive
            }
            else
            {
                // Shrink the object over time
                float scaleFactor = 1f - (meltTimer / meltDuration);
                transform.localScale = initialScale * scaleFactor;

                // Deactivate the meltobject when the block is extremely small
                if (!hasDeactivatedMeltobject && scaleFactor <= 0.1f) // Adjust the threshold as needed
                {
                    if (meltobject != null)
                    {
                        ground.tag = "Untagged";
                        meltobject.SetActive(false);
                        hasDeactivatedMeltobject = true; // Ensure this only happens once
                    }
                }
            }
        }
    }

    void CheckForHeat()
    {
        // Find all objects with the "Fire" tag within the melt radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meltRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Fire"))
            {
                StartMelting();
                break;
            }
        }
    }

    void StartMelting()
    {
        isMelting = true;
        Debug.Log(gameObject.name + " is melting!");

        // Optional: Instantiate a melt effect and store the reference
        if (meltEffect != null)
        {
            spawnedMeltEffect = Instantiate(meltEffect, transform.position, Quaternion.identity);
        }
    }

    void ActivateItems()
    {
        // Activate all items in the itemsToActivate array
        loot.SetActive(false);
    }
}