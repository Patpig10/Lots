using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossdeath : MonoBehaviour
{
    public GameObject targetObject; // The GameObject (Boss) to monitor
    public GameObject emblem; // Object to spawn when boss is destroyed

    private bool hasSpawned = false; // Prevent multiple activations

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned!", this);
            return;
        }

        if (emblem == null)
        {
            Debug.LogError("Emblem object is not assigned!", this);
            return;
        }

        emblem.SetActive(false); // Ensure emblem starts inactive
        StartCoroutine(CheckForDestruction()); // Start monitoring the boss
    }

    private IEnumerator CheckForDestruction()
    {
        while (targetObject != null) // Loop while the target exists
        {
            yield return null; // Wait for the next frame
        }

        // Once the boss is destroyed
        if (!hasSpawned)
        {
            emblem.SetActive(true);
            Debug.Log("Boss destroyed! Emblem activated.");
            hasSpawned = true; // Prevent re-triggering
        }
    }
}
