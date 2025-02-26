using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threedoor : MonoBehaviour
{
    public string requiredKey1;  // First key required to open the door
    public string requiredKey2;  // Second key required to open the door
    public string requiredKey3;  // Third key required to open the door
    public GameObject doorObject; // Reference to the door object
    public GameObject Keybutton;  // Reference to the UI button to open the door
    private Bag playerBag;

    private void Start()
    {
        playerBag = FindObjectOfType<Bag>(); // Finds the Bag component on the player
    }

    public void TryOpenDoor()
    {
        // Check if the player has all three keys
        if (playerBag != null && playerBag.HasItem(requiredKey1) && playerBag.HasItem(requiredKey2) && playerBag.HasItem(requiredKey3))
        {
            // Remove all three keys from the player's bag
            playerBag.RemoveItem(requiredKey1);
            playerBag.RemoveItem(requiredKey2);
            playerBag.RemoveItem(requiredKey3);

            OpenDoor(); // Calls the method to open the door
            Debug.Log("Door opened with " + requiredKey1 + ", " + requiredKey2 + ", and " + requiredKey3 + "!");
        }
        else
        {
            Debug.Log("The door requires " + requiredKey1 + ", " + requiredKey2 + ", and " + requiredKey3 + " keys.");
        }
    }

    private void OpenDoor()
    {
        // Set the door inactive or play an animation
        Destroy(doorObject);
    }

    public void Update()
    {
        // Show the button only if the player has all three keys
        if (playerBag != null && playerBag.HasItem(requiredKey1) && playerBag.HasItem(requiredKey2) && playerBag.HasItem(requiredKey3))
        {
            Keybutton.SetActive(true);
        }
        else
        {
            Keybutton.SetActive(false);
        }
    }
}