using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Townhallzone : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to destroy
    private string saveFilePath;

    [System.Serializable]
    private class ObjectState
    {
        public bool isDestroyed;
    }

    private void Start()
    {
        if (targetObject == null)
        {
            targetObject = gameObject; // Default to the current GameObject if not set
        }

        // Define the save file path for JSON
        saveFilePath = Path.Combine(Application.persistentDataPath, GetUniqueObjectID() + ".json");

        // Check if the GameObject should be destroyed based on saved JSON state
        if (File.Exists(saveFilePath))
        {
            string savedStateJson = File.ReadAllText(saveFilePath);
            ObjectState savedState = JsonUtility.FromJson<ObjectState>(savedStateJson);

            if (savedState.isDestroyed)
            {
                targetObject.SetActive(false);
            }
        }
    }

    public void ResetFairy()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath); // Delete the JSON file to reset the state
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy the GameObject if it enters a trigger collider
        if (other.CompareTag("Trigger")) // Make sure the trigger collider has the tag "Trigger"
        {
            Destroy(targetObject);

            // Save the destroyed state to a JSON file
            ObjectState state = new ObjectState();
            state.isDestroyed = true;
            string jsonState = JsonUtility.ToJson(state);
            File.WriteAllText(saveFilePath, jsonState);
        }
    }

    private string GetUniqueObjectID()
    {
        // Generate a unique ID using the object's name
        return targetObject.name;
    }
}