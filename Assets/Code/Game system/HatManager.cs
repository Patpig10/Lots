using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    public bool Default = true;
    public bool Crown = false;
    public bool Forest = false;
    public bool Funny = false;

    public bool havedefault = true;
    public bool havecrown = false;
    public bool haveforest = false;
    public bool havefunny = false;

    public GameObject DefaultHat;
    public GameObject CrownHat;
    public GameObject ForestHat;
    public GameObject FunnyHat;


    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "hatData.json");
        LoadHats(); // Load hat data when the game starts.
    }

    private void Update()
    {
        if (Default == true)
        {
            Debug.Log("Default hat is equipped");
          
            DefaultHat.SetActive(true);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);

        }
       

        if (Crown == true)
        {
            Debug.Log("Crown hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(true);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);

        }
        
        if (Forest == true)
        {
            Debug.Log("Forest hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(true);
            FunnyHat.SetActive(false);

        }
       
        if (Funny)
        {
            Debug.Log("Funny hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(true);

        }
       
    }

    public void WearHat(string hatName)
    {
        // Reset all hats
        Default = Crown = Forest = Funny = false;

        // Enable the selected hat
        switch (hatName)
        {
            case "Default":
                Default = true;
                break;
            case "Crown":
                Crown = true;
                break;
            case "Forest":
                Forest = true;
                break;
            case "Funny":
                Funny = true;
                break;
                case "havedefault":
                havedefault = true;
                break;
            case "havecrown":
                havecrown = true;
                break;
            case "haveforest":
                haveforest = true;
                break;
            case "havefunny":
                havefunny = true;
                break;

        }

        SaveHats(); // Save the updated hat state.
        Debug.Log($"{hatName} hat is equipped!");
    }

    private void SaveHats()
    {
        HatData data = new HatData
        {
            Default = Default,
            Crown = Crown,
            Forest = Forest,
            Funny = Funny,
            havedefault = havedefault,
            havecrown = havecrown,
            haveforest = haveforest,
            havefunny = havefunny
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Hat data saved to " + saveFilePath);
    }

    private void LoadHats()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            HatData data = JsonUtility.FromJson<HatData>(json);

            Default = data.Default;
            Crown = data.Crown;
            Forest = data.Forest;
            Funny = data.Funny;
            havecrown = data.havecrown;
            havedefault = data.havedefault;
            haveforest = data.haveforest;
            havefunny = data.havefunny;


            Debug.Log("Hat data loaded from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No save file found. Using default hat state.");
        }
    }

    [System.Serializable]
    private class HatData
    {
        public bool Default;
        public bool Crown;
        public bool Forest;
        public bool Funny;
        public bool havedefault;
        public bool havecrown;
        public bool haveforest;
        public bool havefunny;
    }
}


