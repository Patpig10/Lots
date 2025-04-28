using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcegolmAttackHitbox : MonoBehaviour
{
    public int damageAmount = 1;       // Damage to apply
    public float knockbackForce = 5f;  // Knockback force
    public float hitCooldown = 1f;     // Time between allowed hits

    private float lastHitTime = -Mathf.Infinity; // When the last hit happened

    private void OnTriggerEnter(Collider other)
    {
        // If enough time hasn't passed, ignore the hit
        if (Time.time < lastHitTime + hitCooldown)
            return;

        // Check if an unpassable object is inside the hitbox
        if (IsBlockedByUnpassable())
            return;

        // Check if the player entered the hitbox
        if (other.CompareTag("Player"))
        {
            // Update the time of the last hit
            lastHitTime = Time.time;

            // Get the player's ShieldSystem component
            ShieldSystem shieldSystem = other.GetComponent<ShieldSystem>();

            if (shieldSystem != null && shieldSystem.shieldActive)
            {
                // Shield absorbs damage
                shieldSystem.TakeDamageWithShield(damageAmount);
            }
            else
            {
                // Damage the player normally
                HeartSystem playerHealth = other.GetComponent<HeartSystem>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);

                    // Apply knockback
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

    private bool IsBlockedByUnpassable()
    {
        // Check for colliders in the hitbox area
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Unpassable"))
            {
                return true; // Blocked
            }
        }
        return false; // Not blocked
    }

    // Optional: to visualize the hitbox in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
