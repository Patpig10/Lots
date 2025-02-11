using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // The enemy's maximum health
    private int currentHealth; // The enemy's current health
    public Animator animator; // Reference to the Animator component
    public GameObject Body;
    public GameObject damagepoints; // Prefab for the floating text
    public float textDuration = 1f; // How long the floating text stays visible
    public Vector3 textOffset = new Vector3(0, 2, 0); // Offset for the popup text
    public Vector3 Randoml = new Vector3(1, 0, 0);
    public GameObject targetObject; // The GameObject to destroy
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
        ShowFloatingText(damageAmount);

        // Trigger the "Hurt" animation
        animator.SetTrigger("Hurt");

        // Check if the enemy's health has dropped to or below zero
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method when health reaches 0
        }
    }

    // Method to display floating damage text
    public void ShowFloatingText(int damageAmount)
    {
        damagepoints.transform.position += targetObject.transform.position + textOffset;
        var go =  Instantiate (damagepoints, targetObject.transform.position, Quaternion.identity, transform);
        // Calculate the position with the offset

        // damagepoints.transform.position += textOffset;

        damagepoints.transform.localPosition = new Vector3(Random.Range(-Randoml.x, Randoml.x), Random.Range(-Randoml.y, Randoml.y), Random.Range(-Randoml.z, Randoml.z));

        //transform.localPosition = new Vector3(Random.Range(-Randoml.x, Randoml.x), Random.Range(-Randoml.y, Randoml.y), Random.Range(-Randoml.z, Randoml.z));

        // Set the text to display the damage amount
        TextMeshPro textMesh = go.GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textMesh.text = currentHealth.ToString();
            // textMesh.text = damageAmount.ToString();
        }
        else
        {
            Debug.LogError("The damagepoints prefab does not have a TextMeshPro component.");
        }

        // Destroy the floating text after a delay
        Destroy(go, textDuration);
    }

    // Method to handle what happens when the enemy dies
    private void Die()
    {
        Debug.LogError("deadvvvvvvv");
        // For now, we just destroy the enemy GameObject
        Destroy(Body);

        // You could add effects like playing a death animation or dropping items here
    }
}