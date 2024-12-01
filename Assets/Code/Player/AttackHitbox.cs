using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damageAmount = 1;   // Amount of damage to apply to the player
    public float knockbackForce = 5f;  // Optional: force of knockback applied to the player (if needed)

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the hitbox
        if (other.CompareTag("Player"))
        {
            // Get the player's ShieldSystem component
            ShieldSystem shieldSystem = other.GetComponent<ShieldSystem>();

            // If the shield is active, absorb damage
            if (shieldSystem != null && shieldSystem.shieldActive)
            {
                // Absorb damage using the shield's TakeDamageWithShield method
                shieldSystem.TakeDamageWithShield(damageAmount);
            }
            else
            {
                // Get the player's HeartSystem component
                HeartSystem playerHealth = other.GetComponent<HeartSystem>();

                if (playerHealth != null)
                {
                    // Apply damage to the player using their HeartSystem
                    playerHealth.TakeDamage(damageAmount);

                    // Optionally apply knockback to the player (optional if needed)
                    Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
                    if (playerRigidbody != null)
                    {
                        Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                        playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
