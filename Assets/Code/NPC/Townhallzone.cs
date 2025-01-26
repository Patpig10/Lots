using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Townhallzone : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to destroy
    private string saveFilePath;

    private void Start()
    {
        if (targetObject == null)
        {
            targetObject = gameObject; // Default to the current GameObject if not set
        }

        // Define the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, GetUniqueObjectID() + ".txt");

        // Check if the GameObject should be destroyed based on saved state
        if (File.Exists(saveFilePath))
        {
            string savedState = File.ReadAllText(saveFilePath);
            if (savedState == "destroyed")
            {
                Destroy(targetObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the GameObject if it enters a trigger collider
        if (other.CompareTag("Trigger")) // Make sure the trigger collider has the tag "Trigger"
        {
            Destroy(targetObject);

            // Save the destroyed state to a file
            File.WriteAllText(saveFilePath, "destroyed");
        }
    }

    private string GetUniqueObjectID()
    {
        // Generate a unique ID using the object's name
        return targetObject.name;
    }
}
