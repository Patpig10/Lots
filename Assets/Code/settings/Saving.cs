using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Saving : MonoBehaviour
{

    public int maxSavedLife = 3;
    public int weaponSavedDamage = 20;
    public int levelUnlocked = 1;

    private string saveFilePath;

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

    // Method to save player data to JSON
    private void SavePlayerData()
    {
        SaveData saveData = new SaveData
        {
            maxSavedLife = maxSavedLife,
            weaponSavedDamage = weaponSavedDamage,
            levelUnlocked = levelUnlocked
        };

        // Convert the data to JSON format and save it to file
        string jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("Player data saved!");
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
    }
}