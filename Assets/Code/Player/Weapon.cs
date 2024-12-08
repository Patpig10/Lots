using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 20; // The amount of damage the weapon deals

    // This method is triggered when the weapon hits another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object hit has an EnemyHealth component
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        //Dummyhealth dummy = other.GetComponent<Dummyhealth>();
        // If it does, apply damage to the enemy
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Deal damage to the enemy
            return; // Exit the method after dealing damage
        }

        Dummyhealth dummy = other.GetComponent<Dummyhealth>();

        if (dummy != null)
        {
            dummy.TakeDamage(damage); // Deal damage to the enemy
            return; // Exit the method after dealing damage
        }
        // Check if the object hit has a BossSegment component
        BossSegment bossSegment = other.GetComponent<BossSegment>();

        // If it does, apply damage to the boss segment
        if (bossSegment != null)
        {
            bossSegment.TakeDamage(damage); // Deal damage to the boss segment
        }
    }
}
