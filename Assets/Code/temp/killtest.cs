using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killtest : MonoBehaviour
{
    public GameObject[] aa; // Array of GameObjects to check
    public float checkInterval = 0.5f; // Time between checks

    void Start()
    {
        StartCoroutine(CheckForDestruction());
    }

    IEnumerator CheckForDestruction()
    {
        while (true)
        {
            // Only proceed if the array is active
            if (gameObject.activeInHierarchy)
            {
                // Check if all GameObjects in the array are destroyed
                if (AreAllDestroyed(aa))
                {
                    Destroy(gameObject); // Destroy this GameObject
                    yield break; // Exit the coroutine
                }
            }
            yield return new WaitForSeconds(checkInterval); // Wait before checking again
        }
    }

    // Helper method to check if all GameObjects in the array are destroyed
    bool AreAllDestroyed(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            // If any object is still active and not null, return false
            if (obj != null && obj.activeInHierarchy)
            {
                return false;
            }
        }
        // All objects are either destroyed or inactive
        return true;
    }
}
