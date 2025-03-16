using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LibraryTalk : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public int talkCount = 1;
    private bool hasTalked = false;
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "TalkCount.json");
        LoadTalkCount();
        UpdateActiveGameObject(); // Ensure correct state at start
    }

    public void ResetTalking()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        talkCount = 1;
        SaveTalkCount();
        UpdateActiveGameObject();
    }

    public void OnTalkToNPC()
    {
        if (!hasTalked)
        {
            hasTalked = true;
            talkCount++;

            // Ensure talkCount is within bounds
            if (talkCount > gameObjectsToActivate.Length)
            {
                talkCount = gameObjectsToActivate.Length; // Prevent out-of-bounds
            }

            UpdateActiveGameObject();
            SaveTalkCount();
        }
    }

    private void UpdateActiveGameObject()
    {
        // Deactivate all first
        foreach (GameObject go in gameObjectsToActivate)
        {
            go.SetActive(false);
        }

        // Activate only the correct one
        if (talkCount > 0 && talkCount <= gameObjectsToActivate.Length)
        {
            gameObjectsToActivate[talkCount - 1].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTalkToNPC();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasTalked = false;
        }
    }

    private void SaveTalkCount()
    {
        TalkCountData data = new TalkCountData { talkCount = this.talkCount };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadTalkCount()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            TalkCountData data = JsonUtility.FromJson<TalkCountData>(json);
            this.talkCount = data.talkCount;
        }
    }

    [System.Serializable]
    private class TalkCountData
    {
        public int talkCount;
    }
}
