using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Saving : MonoBehaviour
{

    public int maxSavedLife = 3;
    public int weaponSavedDamage = 20;
    public int levelUnlocked = 1;
    public int maxcoins = 0;
    private string saveFilePath;
    public bool completed = false;
    private void Start()
    {
        // Set the file path for the save file
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerSaveData.json");

        // Automatically load player data when the game starts
        LoadPlayerData();
    }

    // Automatically save player data when the application quits
    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    // Setters to save on each stat change
    public void SetMaxLife(int newLife)
    {
        maxSavedLife = newLife;
        SavePlayerData(); // Save data immediately after change
    }

    public void SetWeaponDamage(int newDamage)
    {
        weaponSavedDamage = newDamage;
        SavePlayerData(); // Save data immediately after change
    }

    public void SetLevelUnlocked(int newLevel)
    {
        levelUnlocked = newLevel;
        SavePlayerData(); // Save data immediately after change
    }
    public void OnConnectedToServer(int newCoins)
    {
        maxcoins = newCoins;
        SavePlayerData(); // Save data immediately after change
    }



    // Method to save player data to JSON
    public void SavePlayerData()
    {
        SaveData saveData = new SaveData
        {
            maxSavedLife = maxSavedLife,
            weaponSavedDamage = weaponSavedDamage,
            levelUnlocked = levelUnlocked,
            coins = maxcoins

        };

        // Convert the data to JSON format and save it to file
        string jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("Player data saved!");
    }
    public void LoadLeveltime()
    {
        StartCoroutine(LoadLevelWithDelay(1, 3f)); // Start the coroutine with a 3-second delay
    }
    private IEnumerator LoadLevelWithDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay (3 seconds)

        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex); // Load the scene after the delay
        }
        else
        {
            Debug.LogError("Scene index " + sceneIndex + " out of range!"); // Log an error if the scene index is invalid
        }
    }
    public void Addlevel2()
    {
        if (levelUnlocked > 1)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
            LoadLeveltime();
        }

    }
    public void Addlevel3()
    {
        if (levelUnlocked > 2)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
    public void Addlevel4()
    {
        if (levelUnlocked > 3)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
    // Method to load player data from JSON
    private void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            // Load data into player variables
            maxSavedLife = saveData.maxSavedLife;
            weaponSavedDamage = saveData.weaponSavedDamage;
            levelUnlocked = saveData.levelUnlocked;
            maxcoins = saveData.coins;

            Debug.Log("Player data loaded!");
        }
        else
        {
            Debug.LogWarning("Save file not found! Starting with default values.");
        }
     
}
    [System.Serializable]
    public class SaveData
    {
        public int maxSavedLife;
        public int weaponSavedDamage;
        public int levelUnlocked;
        public int coins;
    }
}
