using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.IO;

public class LibraryTalk : MonoBehaviour
{
    // Array to hold the GameObjects that will be activated based on the talk count
    public GameObject[] gameObjectsToActivate;

    // Variable to keep track of the number of times the player has talked to the NPC
    public int talkCount = 1;

    // Variable to ensure the talk count only increments once per entry
    private bool hasTalked = false;

    // Path to the JSON file where the talk count will be saved
    private string saveFilePath;

    private void Start()
    {
        // Set the save file path (e.g., in the persistent data path)
        saveFilePath = Path.Combine(Application.persistentDataPath, "TalkCount.json");

        // Load the talk count from the JSON file when the game starts
        LoadTalkCount();
    }

    public void ResetTalking()
    {
        // Step 1: Delete the save file if it exists
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        // Step 2: Reset the talk count to the default value
        talkCount = 1;
        // Step 3: Save the default talk count to the JSON file
        SaveTalkCount();
    }

    // Method to be called when the player talks to the NPC
    public void OnTalkToNPC()
    {
        // Increment the talk count only if the player hasn't talked yet in this interaction
        if (!hasTalked)
        {
            talkCount++;
            hasTalked = true; // Prevent repeated increments

            // Deactivate all GameObjects first
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(false);
            }

            // Activate the corresponding GameObject based on the talk count
            if (talkCount > 0 && talkCount <= gameObjectsToActivate.Length)
            {
                gameObjectsToActivate[talkCount - 1].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Talk count is out of range. No GameObject to activate.");
            }

            // Save the updated talk count to the JSON file
            SaveTalkCount();
        }
    }

    // Automatically call OnTalkToNPC when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collider is the player
        {
            OnTalkToNPC(); // Call the talk method
        }
    }

    // Reset the hasTalked flag when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the collider is the player
        {
            hasTalked = false; // Reset the flag for the next interaction
        }
    }

    // Save the talk count to a JSON file
    private void SaveTalkCount()
    {
        // Create a serializable object to store the talk count
        TalkCountData data = new TalkCountData { talkCount = this.talkCount };

        // Convert the object to a JSON string
        string json = JsonUtility.ToJson(data);

        // Write the JSON string to the file
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Talk count saved: " + json);
    }

    // Load the talk count from a JSON file
    private void LoadTalkCount()
    {
        if (File.Exists(saveFilePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(saveFilePath);

            // Convert the JSON string back to a TalkCountData object
            TalkCountData data = JsonUtility.FromJson<TalkCountData>(json);

            // Set the talk count from the loaded data
            this.talkCount = data.talkCount;

            Debug.Log("Talk count loaded: " + json);
        }
        else
        {
            Debug.Log("No save file found. Starting with default talk count.");
        }
    }

    // Serializable class to store the talk count
    [System.Serializable]
    private class TalkCountData
    {
        public int talkCount;
    }
}
