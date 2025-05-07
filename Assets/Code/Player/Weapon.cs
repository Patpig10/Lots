using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 20; // Default damage, will be overridden by saved value
    public bool boom = false;
    public Saving savingSystem;

    private void Start()
    {
        // Find the Saving component in the scene
        savingSystem = FindObjectOfType<Saving>();

        if (savingSystem != null)
        {
            // Set the weapon damage to the saved value
            damage = savingSystem.weaponSavedDamage;
        }
        else
        {
            Debug.LogWarning("Saving system not found! Using default damage value.");
        }
    }

    // This method is triggered when the weapon hits another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object hit has an EnemyHealth component
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        // If it does, apply damage to the enemy
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Deal damage to the enemy
            if(tag=="Block")
            {
                if(boom)
                {
                    enemy.TakeDamage(damage* 3);
                }
                enemy.TakeDamage(damage -= 20);
            }
            return; // Exit the method after dealing damage
        }

        Dummyhealth dummy = other.GetComponent<Dummyhealth>();

        if (dummy != null)
        {
            dummy.TakeDamage(damage); // Deal damage to the dummy
            return; // Exit the method after dealing damage
        }

        FireBossHealth fire = other.GetComponent<FireBossHealth>();

        if (fire != null)
        {
            if (boom)
            {
                fire.TakeDamage(damage*2);
            }
            else
            {
                fire.TakeDamage(damage); // Deal damage to the dummy
            }
           // fire.TakeDamage(damage); // Deal damage to the dummy
            return; // Exit the method after dealing damage
        }

        FireBossPart fireP = other.GetComponent<FireBossPart>();

        if (fireP != null)
        {
            fireP.TakeDamage(damage); // Deal damage to the dummy
            return; // Exit the method after dealing damage
        }

        OrbHealth orb = other.GetComponent<OrbHealth>();

        if (orb != null)
        {
            orb.TakeDamage(damage); // Deal damage to the dummy
            return; // Exit the method after dealing damage
            Debug.Log("Orb hit!");
        }

        
        LichBossHealth Lich = other.GetComponent<LichBossHealth>();

        if (Lich != null)
        {
          

            Lich.ApplyDamage(damage);  // Deal damage to the dummy
            return; // Exit the method after dealing damage
        }

        // Check if the object hit has a BossSegment component
        BossSegment bossSegment = other.GetComponent<BossSegment>();

        // If it does, apply damage to the boss segment
        if (bossSegment != null)
        {
            if (boom)
            {
                bossSegment.TakeDamage(damage);
            }

            else
            {
                bossSegment.TakeDamage(damage); // Deal damage to the boss segment
            }
        }

        IceBossHealth iceBossHealth = other.GetComponent<IceBossHealth>();
        if (iceBossHealth != null)
        {

            if (boom)
            {
                iceBossHealth.ApplyDamage(damage);
            }

            else
            {
                iceBossHealth.ApplyDamage(damage);
            }
        }

       
    }
}