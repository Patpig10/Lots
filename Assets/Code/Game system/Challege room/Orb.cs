using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int collectPoints = 100; // Points awarded for collecting the orb
    public int hitPenaltyPoints = -50; // Points deducted when the orb is hit

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Award points to the player for collecting the orb
            Dodgemanager gameMaster = FindObjectOfType<Dodgemanager>();
            if (gameMaster != null)
            {
                gameMaster.AddScore(collectPoints);
            }

            // Destroy the orb
            Destroy(gameObject);
        }
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shooter"))
        {
            // Deduct points when the orb is hit
            Dodgemanager gameMaster = FindObjectOfType<Dodgemanager>();
            if (gameMaster != null)
            {
                gameMaster.AddScore(hitPenaltyPoints);
            }

            // Destroy the orb
            Destroy(gameObject);
        }
    }*/
}
