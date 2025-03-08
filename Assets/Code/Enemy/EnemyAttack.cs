using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string attackAnimationName = "Attack"; // Name of the attack animation

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider's tag is "Player"
        if (other.CompareTag("Player"))
        {
            // Trigger the attack animation
            if (animator != null)
            {
                animator.SetTrigger(attackAnimationName);
            }
            else
            {
                Debug.LogError("Animator component is not assigned!");
            }
        }
    }
}
