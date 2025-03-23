using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingAnimation : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float attackRange = 3f; // Range within which the enemy will attack
    public Animator animator; // Reference to the Animator component
    public string attackTriggerName = "Attack"; // Name of the attack trigger parameter in the Animator

    void Update()
    {
        // Check if the player is nearby
        if (player != null && IsPlayerNearby())
        {
            // Trigger the attack animation
            animator.SetTrigger(attackTriggerName);
        }
    }

    // Check if the player is within attack range
    private bool IsPlayerNearby()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Return true if the player is within attack range
        return distanceToPlayer <= attackRange;
    }

    // Optional: Visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}