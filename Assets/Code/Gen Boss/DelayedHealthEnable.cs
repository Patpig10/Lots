using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedHealthEnable : MonoBehaviour
{
    public MonoBehaviour healthSystemScript; // Reference to the health system script
    public float delay = 1f; // Delay before enabling the health system

    private void Start()
    {
        if (healthSystemScript == null)
        {
            Debug.LogError("Health system script reference is missing!");
            return;
        }

        // Disable the health system at the start
        healthSystemScript.enabled = false;

        // Start the coroutine to enable it after the delay
        StartCoroutine(EnableHealthSystemAfterDelay());
    }

    private IEnumerator EnableHealthSystemAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Enable the health system
        healthSystemScript.enabled = true;
        Debug.Log("Health system enabled after delay.");
    }
}
