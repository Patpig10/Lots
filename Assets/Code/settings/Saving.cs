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
    public bool isShootUnlocked = false;
    public bool isShieldUnlocked = false;
    public bool isAoEUnlocked = false;
    public bool Grassemblem = false;
    public bool Iceemblem = false;
    public bool Fireemblem = false;
    public bool ice;
    public bool fire;
    public bool Arena;
    public bool Arenaclear;
    // Default values for reset
    private const int DEFAULT_MAX_LIFE = 3;
    private const int DEFAULT_WEAPON_DAMAGE = 20;
    private const int DEFAULT_LEVEL_UNLOCKED = 1;
    private const int DEFAULT_COINS = 0;
    private const bool DEFAULT_SHOOT_UNLOCKED = false;
    private const bool DEFAULT_SHIELD_UNLOCKED = false;
    private const bool DEFAULT_AOE_UNLOCKED = false;
    public const bool DEFAULT_GRASSEMBLEM = false;
    public const bool DEFAULT_ice = false;
    public const bool DEFAULT_fire = false;
    public const bool DEFAULT_arena = false;
    public const bool DEFAULT_arenaclear = false;
    public const bool DEFAULT_IceEMBLEM = false;
    public const bool DEFAULT_FireEMBLEM = false;





    private void Start()
    {
        // Set the file path for the save file
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerSaveData.json");

        // Automatically load player data when the game starts
        LoadPlayerData();
    }

    // Reset function
    public void ResetGame()
    {
        // Step 1: Delete the save file if it exists
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
        }

        // Step 2: Reset all player data to default values
        maxSavedLife = DEFAULT_MAX_LIFE;
        weaponSavedDamage = DEFAULT_WEAPON_DAMAGE;
        levelUnlocked = DEFAULT_LEVEL_UNLOCKED;
        maxcoins = DEFAULT_COINS;
        isShootUnlocked = DEFAULT_SHOOT_UNLOCKED;
        isShieldUnlocked = DEFAULT_SHIELD_UNLOCKED;
        isAoEUnlocked = DEFAULT_AOE_UNLOCKED;
        Grassemblem = DEFAULT_GRASSEMBLEM;
        fire = DEFAULT_fire;
        ice = DEFAULT_ice;
        Arena = DEFAULT_arena;
        Arenaclear = DEFAULT_arenaclear;


        // Step 3: Save the default values to a new save file
        SavePlayerData();
        Debug.Log("Game reset to default values!");
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


    public void shootunlock()
    {
        isShootUnlocked = true;
        SavePlayerData();
    }
    public void Grassemblemunlock()
    {
        Grassemblem = true;
        SavePlayerData();
    }
    public void Iceemblemunlock()
    {
        Iceemblem = true;
        SavePlayerData();
    }
    public void Fireemblemunlock()
    {
        Fireemblem = true;
        SavePlayerData();
    }

    public void shieldunlock()
    {
        isShieldUnlocked = true;
        SavePlayerData();
    }

    public void aoeunlock()
    {
        isAoEUnlocked = true;
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
            coins = maxcoins,
            isAoEUnlocked = isAoEUnlocked,
            isShootUnlocked = isShootUnlocked,
            isShieldUnlocked = isShieldUnlocked,
            Grassemblem = Grassemblem,
            ice = ice,
            fire = fire,
            Arena = Arena,
            Arenaclear = Arenaclear,
            Iceemblem = Iceemblem,
            Fireemblem = Fireemblem

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
        if (levelUnlocked == 1)
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

    public void ToVillage()
    {
        SceneManager.LoadScene(6);
    }

    public void Addlevel3()
    {
        if (levelUnlocked == 2)
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
        if (levelUnlocked == 3)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
    public void Addlevel5()
    {
        if (levelUnlocked == 4)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
      public void Addlevelice1()
    {
        if (levelUnlocked == 5)
        {
            completed = true;
        }
        if (completed)
        {
            ice = true;
            levelUnlocked++;
            SavePlayerData();
        }
    }
    public void Addlevelice2()
    {
        if (levelUnlocked == 6)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
    public void Addleveliceboss()
    {
        if (levelUnlocked == 7)
        {
            completed = true;
        }
        if (completed)
        {
            levelUnlocked++;
            SavePlayerData();
        }
    }
    public void Addlevelfire1()
    {
        if (levelUnlocked == 5)
        {
            completed = true;
        }
        if (completed)
        {
            fire = true;
            Arena = true;

            SavePlayerData();
        }
    }

    public void Addlevelfireboss()
    {
        if (Arena == true)
        {
            completed = true;
        }
        if (completed)
        {
            Arenaclear = true;
            SavePlayerData();
        }
    }

    // Method to load player data from JSON
    public void LoadPlayerData()
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
            isAoEUnlocked = saveData.isAoEUnlocked;
            isShootUnlocked = saveData.isShootUnlocked;
            isShieldUnlocked = saveData.isShieldUnlocked;
            Grassemblem = saveData.Grassemblem;
            ice = saveData.ice;
            fire = saveData.fire;
            Arena = saveData.Arena;
            Arenaclear = saveData.Arenaclear;
            Iceemblem = saveData.Iceemblem;
            Fireemblem = saveData.Fireemblem;
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
        public bool isAoEUnlocked;
        public bool isShootUnlocked;
        public bool isShieldUnlocked;
        public bool Grassemblem;
        public bool ice;
        public bool fire;
        public bool Arena;
        public bool Arenaclear;
        public bool Iceemblem;
        public bool Fireemblem;
    }
}