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
    public TextMeshProUGUI coinsText;  // Reference to the UI text displaying the player's coins
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinsText.text = "Coins: " + coins.ToString();  // Update the UI text to display the player's coins
    }

    public void Heal()
    {
        cost = 4;
        if (coins >= cost)
        {
            heartSystem.AddLife(1);  // Call the AddHeart method from the HeartSystem script
            coins -= cost;  // Subtract the cost from the player's coins
        }
    }

    public void UpgradeWeapon()
    {
        cost = 5;
        if (coins >= cost)
        {
            weapon.damage += 10;  // Increase the damage of the weapon
            coins -= cost;  // Subtract the cost from the player's coins
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;  // Add the specified amount of coins to the player's total
    }

    public void Addhearts()
    {
        cost = 4;
        if (coins >= cost)
        {
            heartSystem.UpgradeMaxLife(1);  // Call the AddHeart method from the HeartSystem script
            heartSystem.AddLife(1);
            coins -= cost;  // Subtract the cost from the player's coins
        }
    }
}