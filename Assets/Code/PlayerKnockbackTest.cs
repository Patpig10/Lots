using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackTest : MonoBehaviour
{
    public float movementSpeed = 5f;       // Movement speed for knockback
    public int knockbackDistance = 2;      // How many grid blocks to knock the player back
    public GridMovement gridMovement;      // Reference to the existing GridMovement script

    private bool isKnockedBack = false;    // Check if the player is currently being knocked back

    void Update()
    {
        // Testing knockback with the "K" key
        if (Input.GetKeyDown(KeyCode.K) && !isKnockedBack)
        {
            Vector3 knockbackDirection = -transform.forward;  // Knockback in the opposite direction of player's forward
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a projectile
        if (other.CompareTag("Bullet") && !isKnockedBack)
        {
            // Calculate knockback direction based on projectile's position relative to player
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;

            // Start knockback coroutine
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance));
        }
    }

    public IEnumerator ApplyKnockback(Vector3 direction, int distance)
    {
        isKnockedBack = true;

        for (int i = 0; i < distance; i++)
        {
            // Use FindNearestBlock method from GridMovement to find the next block in the knockback direction
            if (gridMovement.FindNearestBlock(direction))
            {
                // Move to the block using the existing MoveToBlock logic
                yield return StartCoroutine(gridMovement.MoveToBlock());
            }
            else
            {
                // If no valid block is found, break the loop
                break;
            }
        }

        isKnockedBack = false;
    }
}
