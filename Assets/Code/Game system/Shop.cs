using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public HeartSystem heartSystem;  // Reference to the HeartSystem script
    public Weapon weapon;  // Reference to the Weapon script
    public int cost;
    public int coins;
    public int Attack;  
    public TextMeshProUGUI coinsText;  // Reference to the UI text displaying the player's coins
    public TextMeshProUGUI AttackText;
    public Saving save;

    // Start is called before the first frame update
    void Start()
    {
        save = GameObject.FindObjectOfType<Saving>();
        coins = save.maxcoins;  // Set the player's coins to the amount saved in the SaveData object

    }

    // Update is called once per frame
    void Update()
    {
        Attack = weapon.damage;
        coins = save.maxcoins;  // Set the player's coins to the amount saved in the SaveData object
        AttackText.text = "Attack: " + Attack.ToString();  // Update the UI text to display the player's coins
        coinsText.text = "Coins: " + coins.ToString();  // Update the UI text to display the player's coins
    }

    public void Heal()
    {
        cost = 60;
        if (coins >= cost)
        {
            heartSystem.AddLife(1);  // Call the AddHeart method from the HeartSystem script
            coins -= cost;  // Subtract the cost from the player's coins
            save.maxcoins = coins;
        }
    }

    public void UpgradeWeapon()
    {
        cost = 150;
        if (coins >= cost)
        {
            weapon.damage += 20;  // Increase the damage of the weapon
            save.SetWeaponDamage(weapon.damage);  // Update the saved damage value
            coins -= cost;  // Subtract the cost from the player's coins
            save.maxcoins = coins;  // Update the saved coins value
            save.SavePlayerData();  // Save the updated data
        }
    }
    public void AddCoins(int amount)
    {
        coins += amount;  // Add the specified amount of coins to the player's total
        save.maxcoins = coins;  // Update the saved coins value
        save.SavePlayerData();
    }

    public void Addhearts()
    {
        cost = 250;
        if (coins >= cost)
        {
            heartSystem.UpgradeMaxLife(1);  // Call the AddHeart method from the HeartSystem script
            heartSystem.AddLife(1);
            coins -= cost;  // Subtract the cost from the player's coins
            save.maxcoins = coins;
            save.SavePlayerData();

        }
    }
}
