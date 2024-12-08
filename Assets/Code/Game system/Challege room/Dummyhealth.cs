using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummyhealth : MonoBehaviour
{

    public int maxHealth = 1000; // The enemy's maximum health
    private int currentHealth; // The enemy's current health
    public Animator animator; // Reference to the Animator component
    public GameObject Body;
    // public BoxCollider BoxCollider; // Reference to the BoxCollider component

    void Start()
    {
        // Initialize the enemy's health to the maximum value at the start
        currentHealth = maxHealth;

        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();
    }

    // This function will be called when the enemy takes damage
    public void TakeDamage(int damageAmount)
    {
        // Reduce the enemy's current health by the damage amount
        currentHealth -= damageAmount;

        // Trigger the "Hurt" animation
        animator.SetTrigger("Hurt");

        // Check if the enemy's health has dropped to or below zero
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method when health reaches 0
        }
    }

    // Method to handle what happens when the enemy dies
    private void Die()
    {
        // For now, we just destroy the enemy GameObject
        Destroy(Body);

        // You could add effects like playing a death animation or dropping items here
    }
}
