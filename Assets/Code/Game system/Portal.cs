using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destinationPortal;   // Assign the destination portal in the Inspector
    public Transform player;              // Assign the player transform in the Inspector (the child object)


    public void Awake()
    {
        //find the player object
        player = GameObject.Find("Player").transform;
    }


    public void TeleportPlayer()
    {
        if (destinationPortal != null && player != null)
        {
            // Move the player's parent to the destination portal's position
            Transform playerParent = player.parent != null ? player.parent : player;  // Check if the player has a parent
            playerParent.position = new Vector3(destinationPortal.position.x, playerParent.position.y, destinationPortal.position.z);
            Debug.Log("Player's parent teleported to: " + destinationPortal.position);
        }
        else
        {
            Debug.LogWarning("Destination portal or player not set for " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only teleport if the specific player (or its parent) enters the portal
        if (other.transform == player)
        {
            TeleportPlayer();
        }
    }
}