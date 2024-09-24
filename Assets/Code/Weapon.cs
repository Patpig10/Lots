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

        // If it does, apply damage to the enemy
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Deal damage to the enemy
        }
    }
}
