using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class MeltableObject : MonoBehaviour
{
    public float meltRadius = 5f; // Radius within which the heat can melt this object
    public float meltDuration = 3f; // Time it takes for the object to melt completely
    public GameObject meltEffect; // Optional: Visual effect for melting
    public GameObject[] itemsToActivate; // Array of inactive items to activate once melted
    public GameObject loot;
    private bool isMelting = false;
    private float meltTimer = 0f;
    private GameObject spawnedMeltEffect; // Reference to the spawned melt effect
    private Vector3 initialScale; // Initial scale of the object

    void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;
        // Activate all items in the itemsToActivate array

       // loot.SetActive(false);
        //after 1 second setActive to true
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
                   // Destroy(spawnedMeltEffect);
                }

                // Activate the inactive items
                ActivateItems();
                loot.SetActive(true);
                gameObject.SetActive(false); // Destroy the object after melting
            }
            else
            {
                // Shrink the object over time
                float scaleFactor = 1f - (meltTimer / meltDuration);
                transform.localScale = initialScale * scaleFactor;
            }
        }
    }

    void CheckForHeat()
    {
        // Find all objects with the "Heat" tag within the melt radius
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