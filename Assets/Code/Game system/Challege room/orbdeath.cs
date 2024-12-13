using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbdeath : MonoBehaviour
{
    public int collectPoints = 100; // Points awarded for collecting the orb
    public int hitPenaltyPoints = -50; // Points deducted when the orb is hit
    public float speed = 10f;                // Speed of the projectile
    public float lifetime = 5f;              // Time before the projectile is destroyed
    public bool destroyOnCollision = true;   // Whether to destroy the projectile on collision
    public int knockbackDistance = 2;        // The number of grid cells to knock back the player
    public float knockbackStrength = 5f;     // The strength of the knockback (speed of knockback movement)
    public bool player = false;              // Whether the projectile is a player projectile
    void Start()
    {
        // Destroy the projectile after 'lifetime' seconds to prevent it from existing indefinitely
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward in the direction it's facing
        MoveForward();
    }

    // Move the projectile forward along its local forward axis
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Handle trigger collisions (e.g., hitting the player)
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Test -50");
            // Deduct points when the orb is hit
            Dodgemanager gameMaster = FindObjectOfType<Dodgemanager>();
            if (gameMaster != null)
            {
                gameMaster.AddScore(hitPenaltyPoints);
            }

            // Destroy the orb
            Destroy(gameObject);
        }
    
        if (other.CompareTag("Unpassable"))
        {
            Destroy(gameObject);

        }
        // Check if the collided object is the player
       
        else if (destroyOnCollision)
        {
            // Optionally, destroy the projectile upon entering a trigger collider that's not the player
            //Destroy(gameObject, 0.1f);
        }

       // Destroy(gameObject) after 0.4 second
        Destroy(gameObject, 0.1f);

       


    }

    void OnCollisionEnter(Collision collision)
    {
        // Optionally, destroy the projectile upon collision with any object
        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

  
}
