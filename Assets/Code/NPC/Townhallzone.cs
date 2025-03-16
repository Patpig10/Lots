using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Townhallzone : MonoBehaviour
{
    public GameObject targetObject; // The object to disable instead of destroy
    private static string saveFilePath;

    [System.Serializable]
    private class ObjectState
    {
        public bool isDestroyed;
    }

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "TownhallState.json");

        // If this is the scene where targetObject exists, load the state
        if (targetObject != null)
        {
            LoadState();
        }
    }

    /// <summary>
    /// Loads the destruction state of the object and disables it if needed.
    /// </summary>
    private void LoadState()
    {
        if (File.Exists(saveFilePath))
        {
            string savedStateJson = File.ReadAllText(saveFilePath);
            ObjectState savedState = JsonUtility.FromJson<ObjectState>(savedStateJson);

            if (savedState.isDestroyed)
            {
                targetObject.SetActive(false);
                Debug.Log("Object was previously destroyed, keeping it disabled.");
            }
            else
            {
                targetObject.SetActive(true);
                Debug.Log("Object should be active.");
            }
        }
        else
        {
            Debug.Log("No save file found, assuming object should be active.");
            targetObject.SetActive(true);
        }
    }

    /// <summary>
    /// Call this from any scene to reset the destruction state.
    /// </summary>
    public static void ResetFairy()
    {
        Debug.Log("Resetting object...");

        // Reset state in JSON
        ObjectState state = new ObjectState { isDestroyed = false };
        string jsonState = JsonUtility.ToJson(state, true);
        File.WriteAllText(saveFilePath, jsonState);

        Debug.Log("Object reset. It will be reactivated in its scene.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            targetObject.SetActive(false);
            SaveState(true);
        }
    }

    private void SaveState(bool isDestroyed)
    {
        ObjectState state = new ObjectState { isDestroyed = isDestroyed };
        string jsonState = JsonUtility.ToJson(state, true);
        File.WriteAllText(saveFilePath, jsonState);

        Debug.Log("Saved State: " + jsonState);
    }
}
