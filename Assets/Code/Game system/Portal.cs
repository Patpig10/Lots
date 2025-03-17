using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Portal : MonoBehaviour
{
    public Transform destinationPortal;   // Assign the destination portal in the Inspector
    public Transform player;              // Assign the player transform in the Inspector (the child object)

    private bool isTeleporting = false;   // Flag to prevent multiple triggers

    public void Awake()
    {
        // Find the player object
        player = GameObject.Find("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }
        else
        {
            Debug.Log("Player found: " + player.name);
        }
    }

    public void TeleportPlayer()
    {
        if (destinationPortal == null)
        {
            Debug.LogError("Destination portal not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player not assigned!");
            return;
        }

        // Move the player's parent to the destination portal's position
        Transform playerParent = player.parent != null ? player.parent : player;
        playerParent.position = new Vector3(destinationPortal.position.x, playerParent.position.y, destinationPortal.position.z);
        Debug.Log("Player's parent teleported to: " + destinationPortal.position);

        // Reset the teleporting flag after a short delay
        StartCoroutine(ResetTeleportCooldown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;

        if (other.CompareTag("Player"))
        {
            isTeleporting = true;
            TeleportPlayer();
        }
    }

    private IEnumerator ResetTeleportCooldown()
    {
        yield return new WaitForSeconds(1f); // Adjust the cooldown duration as needed
        isTeleporting = false;
    }
}