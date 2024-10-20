using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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
        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            if (player == false)
            {

                // Try to get the PlayerKnockbackTest script attached to the player
                PlayerKnockbackTest playerKnockback = other.GetComponent<PlayerKnockbackTest>();



                if (playerKnockback != null)
                {
                    // Calculate knockback direction (away from the projectile)
                    Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

                    // Apply knockback to the player
                    // StartCoroutine(playerKnockback.ApplyKnockback(knockbackDirection, knockbackDistance));
                }
            }
            // Destroy the projectile upon hitting the player
            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
        else if (destroyOnCollision)
        {
            // Optionally, destroy the projectile upon entering a trigger collider that's not the player
            Destroy(gameObject);
        }
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
