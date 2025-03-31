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
    public bool Fancy = false;
    public bool Forge = false;
    public bool sleep = false;

    public bool havedefault = true;
    public bool havecrown = false;
    public bool haveforest = false;
    public bool havefunny = false;
    public bool havefancy = false;
    public bool haveforge = false;
    public bool havesleep = false;

    public GameObject DefaultHat;
    public GameObject CrownHat;
    public GameObject ForestHat;
    public GameObject FunnyHat;
    public GameObject FancyHat;
    public GameObject ForgeHat;
    public GameObject SleepHat;


    private string saveFilePath;

    public void ResetHats()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        Default = true;
        Crown = false;
        Forest = false;
        Funny = false;
        Fancy = false;
        Forge = false;
        sleep = false;
        havedefault = true;
        havecrown = false;
        haveforest = false;
        havefunny = false;
        havesleep = false;
        havefancy = false;
        haveforge = false;
        havesleep = false;
        SaveHats();
    }
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
            FancyHat.SetActive(false);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(false);

        }
       

        if (Crown == true)
        {
            Debug.Log("Crown hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(true);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);
            FancyHat.SetActive(false);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(false);


        }
        
        if (Forest == true)
        {
            Debug.Log("Forest hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(true);
            FunnyHat.SetActive(false);
            FancyHat.SetActive(false);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(false);


        }
       
        if (Funny == true)
        {
            Debug.Log("Funny hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(true);
            FancyHat.SetActive(false);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(false);

        }
        if (Fancy == true)
        {
            Debug.Log("Fancy hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);
            FancyHat.SetActive(true);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(false);
        }
        if (Forge == true)
        {
            Debug.Log("Forge hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);
            FancyHat.SetActive(false);
            ForgeHat.SetActive(true);
            SleepHat.SetActive(false);
        }
        if (sleep == true)
        {
            Debug.Log("Sleep hat is equipped");
            DefaultHat.SetActive(false);
            CrownHat.SetActive(false);
            ForestHat.SetActive(false);
            FunnyHat.SetActive(false);
            FancyHat.SetActive(false);
            ForgeHat.SetActive(false);
            SleepHat.SetActive(true);
        }

    }

    public void WearHat(string hatName)
    {
        // Reset all hats
        Default = Crown = Forest = Funny = sleep = Fancy = Forge = false;

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
            case "Fancy":
                Fancy = true;
                break;
            case "Forge":
                Forge = true;
                break;
            case "Sleep":
                sleep = true;
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
            case "havefancy":
                break;
            case "haveforge":
                break;
            case "havesleep":
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
            sleep = sleep,
            fancy = Fancy,
            forge = Forge,
            havedefault = havedefault,
            havecrown = havecrown,
            haveforest = haveforest,
            havefunny = havefunny,
            havesleep = havesleep,
            havefancy = havefancy,
            haveforge = haveforge

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
            sleep = data.sleep;
            Fancy = data.fancy;
            Forge = data.forge;
            havecrown = data.havecrown;
            havedefault = data.havedefault;
            haveforest = data.haveforest;
            havefunny = data.havefunny;
            havecrown = data.havecrown;
            havesleep = data.havesleep;
            havefancy = data.havefancy;
            haveforge = data.haveforge;



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
        public bool sleep;
        public bool fancy;
        public bool forge;
        public bool havesleep;
        public bool havefancy;
        public bool haveforge;
        public bool havedefault;
        public bool havecrown;
        public bool haveforest;
        public bool havefunny;

    }
}


