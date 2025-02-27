using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public string itemName; // Set this to the name of the item in the Inspector
    private Bag playerBag;
    public int healAmount = 1;
    public HeartSystem heartSystem;
    public Shop Shop;
    public int Coins;
    public Saving save;
    public int Key;
    public Keytraker Keytraker;
    public bool multipleKeys  = false;
    private void Start()
    {
       // Key = Keytraker.keysLeft;
        playerBag = FindObjectOfType<Bag>(); // Finds the Bag component on the player
    }

    public void Awake()
    {
        save = GameObject.FindObjectOfType<Saving>();
  
        heartSystem = GameObject.FindObjectOfType<HeartSystem>();
        Shop = GameObject.FindObjectOfType<Shop>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBag.AddItem(itemName); // Add the item to the bag
                                         //   PopupManager.Instance.ShowPopup($"{itemName} added to bag!"); // Show popup
            if (itemName == "Potion")
            {
                heartSystem.AddLife(healAmount);
                PopupManager.Instance.ShowPopup($"You got a {itemName}. You healed {healAmount} hearts"); // Show popup


            }
            if (itemName == "Coin")
            {
                Shop.AddCoins(Coins);
                PopupManager.Instance.ShowPopup($"You got a {itemName}. You got {Coins} Gold"); // Show popup

            }
            if (itemName == "Key")
            {

                PopupManager.Instance.ShowPopup($"You got a {itemName}. You can now open the door"); // Show popup
            }
            if (itemName == "Emeblem of life")
            {
                PopupManager.Instance.ShowPopup($"Oh great hero I give you the {itemName}."); // Show popup
            }

            Destroy(gameObject); // Destroy the item GameObject
            if (itemName == "Key1")
            {
               Keytraker.keysLeft--;

                PopupManager.Instance.ShowPopup($"You got a {itemName}. You only need to get {Keytraker.keysLeft} more keys");

            }

            if (itemName == "Key2")
            {
               Keytraker.keysLeft--;

                PopupManager.Instance.ShowPopup($"You got a {itemName}. You only need to get {Keytraker.keysLeft} more keys");

            }

            if (itemName == "Key3")
            {
               Keytraker.keysLeft--;

                PopupManager.Instance.ShowPopup($"You got a {itemName}. You only need to get {Keytraker.keysLeft} more keys");
            }

            if (itemName == "Milk")
            {
                Keytraker.keysLeft--;

                PopupManager.Instance.ShowPopup($"You got Milk. Return to the Chef.");
            }
        }
    }

   /* public void Update()
    {
        // If item name is "Potion" and player grabs it, heal player one heart or int amount
        if (itemName == "Potion")
        {
            heartSystem.AddLife(healAmount);


        }

        if (itemName == "Coin")
        {
            Shop.AddCoins(Coins);
        }


    }*/
}
