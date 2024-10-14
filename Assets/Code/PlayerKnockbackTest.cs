using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackTest : MonoBehaviour
{
    public int knockbackDistance = 2;      // Distance to knock back the player
    public GridMovement gridMovement;      // Reference to the existing GridMovement script
    public float knockbackCooldown = 3f;   // Time before the player can move again after knockback

    private bool isKnockedBack = false;    // Check if the player is currently being knocked back

    void Update()
    {
        // Testing knockback with the "K" key
        if (Input.GetKeyDown(KeyCode.K) && !isKnockedBack)
        {
            // Knockback in the opposite direction of the player's current forward direction
            Vector3 knockbackDirection = -transform.forward;
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance, 0f)); // No delay for test
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a bullet
        if (other.CompareTag("Bullet")|| other.CompareTag("Weapon") && !isKnockedBack)
        {
            // Calculate knockback direction based on bullet's position relative to the player
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance, knockbackCooldown));
        }
    }

    // Apply knockback with a cooldown before movement is allowed again
    public IEnumerator ApplyKnockback(Vector3 direction, int distance, float cooldownTime)
    {
        isKnockedBack = true;

        for (int i = 0; i < distance; i++)
        {
            // Call the Knockback method from GridMovement
            gridMovement.Knockback(1, direction); // Knockback by 1 block each time in the given direction
            yield return new WaitForSeconds(0.1f); // Adjust timing if needed
        }

        // Wait for the cooldown period (3 seconds if hit by a bullet)
        yield return new WaitForSeconds(cooldownTime);

        isKnockedBack = false;  // Allow player movement again after the cooldown
    }
}
