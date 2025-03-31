using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HatShop : MonoBehaviour
{
    public int coins; // Player's current coins
    public TextMeshProUGUI coinsText; // UI text to display coins
    public HatManager hatManager; // Reference to the HatManager
    public Saving save; // Reference to the Saving system

    // Different costs for each hat
    public int crownCost = 30000;
    public int forestCost = 350;
    public int funnyCost = 1000000;
    public int sleepCost = 100;
    public int fancyCost = 100;
    public int forgeCost = 100;



    private void Start()
    {
        // Initialize references
        save = GameObject.FindObjectOfType<Saving>();
        hatManager = GameObject.FindObjectOfType<HatManager>();

        // Load the player's coin balance
        coins = save.maxcoins;
        UpdateCoinsUI();
    }

    void Update()
    {
        coins = save.maxcoins;  // Set the player's coins to the amount saved in the SaveData object
        coinsText.text = "Coins: " + coins.ToString();  // Update the UI text to display the player's coins
        UpdateCoinsUI();


    }

    // Update the UI to display the current coin balance
    private void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins.ToString();
        }
    }

    // Method to buy a hat
    public void BuyHat(string hatName, int hatCost)
    {
        if (coins >= hatCost)
        {
            switch (hatName)
            {
                case "Crown":
                    if (!hatManager.havecrown)
                    {
                        hatManager.havecrown = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Crown hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Crown hat!");
                    }
                    break;

                case "Forest":
                    if (!hatManager.haveforest)
                    {
                        hatManager.haveforest = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Forest hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Forest hat!");
                    }
                    break;

                case "Funny":
                    if (!hatManager.havefunny)
                    {
                        hatManager.havefunny = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Funny hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Funny hat!");
                    }
                    break;

                    case "Sleep":
                    if (!hatManager.havesleep)
                    {
                        hatManager.havesleep = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Sleep hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Sleep hat!");
                    }
                    break;

                    case "Fancy":
                    if (!hatManager.havefancy)
                    {
                        hatManager.havefancy = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Fancy hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Fancy hat!");
                    }
                    break;

                    case "Forge":
                    if (!hatManager.haveforge)
                    {
                        hatManager.haveforge = true;
                        coins -= hatCost; // Deduct the cost of the hat
                        save.maxcoins = coins; // Update the saved coin balance
                        save.SavePlayerData(); // Save the updated data
                        Debug.Log("Forge hat purchased!");
                    }
                    else
                    {
                        Debug.Log("You already own the Forge hat!");
                    }
                    break;
                default:
                    Debug.Log("Invalid hat name!");
                    break;
            }

            UpdateCoinsUI(); // Update the UI after purchasing
        }
        else
        {
            Debug.Log("Not enough coins to buy the " + hatName + " hat!");
        }
    }

    // Wrapper methods for Unity buttons
    public void BuyCrownHat()
    {
        BuyHat("Crown", crownCost);
    }

    public void BuyForestHat()
    {
        BuyHat("Forest", forestCost);
    }

    public void BuyFunnyHat()
    {
        BuyHat("Funny", funnyCost);
    }

    public void BuySleepHat()
    {
        BuyHat("Sleep", sleepCost);
    }

    public void BuyFancyHat()
    {
        BuyHat("Fancy", fancyCost);
    }

    public void BuyForgeHat()
    {
        BuyHat("Forge", forgeCost);
    }

    // Method to equip a hat
    public void EquipHat(string hatName)
    {
        switch (hatName)
        {
            case "Default":
                hatManager.WearHat(hatName);
                Debug.Log("Default hat equipped!");
                break;

            case "Crown":
                if (hatManager.havecrown)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Crown hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Crown hat!");
                }
                break;

            case "Forest":
                if (hatManager.haveforest)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Forest hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Forest hat!");
                }
                break;

            case "Funny":
                if (hatManager.havefunny)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Funny hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Funny hat!");
                }
                break;

            case "Sleep":
                if (hatManager.havesleep)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Sleep hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Sleep hat!");
                }
                break;

                case "Fancy":
                if (hatManager.havefancy)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Fancy hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Fancy hat!");
                }
                break;
                case "Forge":
                if (hatManager.haveforge)
                {
                    hatManager.WearHat(hatName);
                    Debug.Log("Forge hat equipped!");
                }
                else
                {
                    Debug.Log("You don't own the Forge hat!");
                }
                break;

            default:
                Debug.Log("Invalid hat name!");
                break;
        }
    }

    // Method to add coins (for testing or rewards)
    public void AddCoins(int amount)
    {
        coins += amount;
        save.maxcoins = coins;
        save.SavePlayerData();
        UpdateCoinsUI();
        Debug.Log($"Added {amount} coins. Total coins: {coins}");
    }
}