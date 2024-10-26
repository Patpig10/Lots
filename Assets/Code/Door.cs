using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredKey;   // Specify which key is required in the Inspector
    public GameObject doorObject; // Reference to the door object

    private Bag playerBag;

    private void Start()
    {
        playerBag = FindObjectOfType<Bag>(); // Finds the Bag component on the player
    }

    public void TryOpenDoor()
    {
        if (playerBag != null && playerBag.HasItem(requiredKey))
        {
            playerBag.RemoveItem(requiredKey); // Removes the key from the bag
            OpenDoor(); // Calls the method to open the door
            Debug.Log("Door opened with " + requiredKey + "!");
        }
        else
        {
            Debug.Log("The door requires a " + requiredKey + " key.");
        }
    }

    private void OpenDoor()
    {
        // Set the door inactive or play an animation
        doorObject.SetActive(false); // Deactivate the door object
    }


}
