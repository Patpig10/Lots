using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burnscene : MonoBehaviour
{
    public Animator playerAnimator; // Public Animator variable


    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            // Get the Animator component from the player

            if (playerAnimator != null)
            {
                // Set the "burn" boolean to true
                playerAnimator.SetBool("burn", true);
            }
        }
    }

  /*  private void OnTriggerExit(Collider other)
    {
        // Reset the "burn" boolean when the player leaves the trigger
        if (other.CompareTag("Player"))
        {
            Animator playerAnimator = other.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("burn", false);
            }
        }
    }*/
}