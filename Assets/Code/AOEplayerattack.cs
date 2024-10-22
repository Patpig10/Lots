using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEplayerattack : MonoBehaviour
{
    public int damage = 20; // The amount of damage the weapon deals
    public float maxBlastRadius = 5f; // The maximum radius of the AOE blast
    public float expansionDuration = 1.5f; // Time it takes for the blast to fully expand
    public GameObject fireAOEPrefab; // Reference to the Fire AOE prefab (particle system)
    public Transform playerTransform; // Reference to the player's transform

    // A list to keep track of enemies that have already been hit
    private List<EnemyHealth> enemiesHit = new List<EnemyHealth>();

    // Update is called once per frame
    private void Update()
    {
        // Check if the player presses the 'T' key
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Clear the list of previously hit enemies before starting a new AOE attack
            enemiesHit.Clear();

            // Start the expanding AOE attack
            StartCoroutine(ExpandingAOEAttack());

            // Instantiate the Fire AOE effect at the player's position
            PlayFireAOEEffect();
        }
    }

    public void AOE()
    {
        enemiesHit.Clear();

        StartCoroutine(ExpandingAOEAttack());
        PlayFireAOEEffect();


    }

    // Coroutine to handle the expanding AOE attack over time
    public IEnumerator ExpandingAOEAttack()
    {
        float currentRadius = 0f; // Start with a radius of 0
        float timeElapsed = 0f;

        while (timeElapsed < expansionDuration)
        {
            // Gradually increase the radius over time
            currentRadius = Mathf.Lerp(0f, maxBlastRadius, timeElapsed / expansionDuration);

            // Apply damage to enemies within the current radius
            DamageEnemiesInRadius(currentRadius);

            // Wait for the next frame
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the AOE reaches the maximum radius at the end
        DamageEnemiesInRadius(maxBlastRadius);
    }

    // This method finds and damages enemies within the given radius
    private void DamageEnemiesInRadius(float radius)
    {
        // Find all colliders within the radius
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, radius);

        // Loop through all hit colliders
        foreach (Collider hit in hitColliders)
        {
            // Check if the object hit has an EnemyHealth component
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();

            // If it does, and the enemy has not already been hit
            if (enemy != null && !enemiesHit.Contains(enemy))
            {
                // Apply damage to the enemy
                enemy.TakeDamage(damage);

                // Add the enemy to the list of hit enemies
                enemiesHit.Add(enemy);
            }
        }
    }

    // Method to play the fire AOE visual effect
    private void PlayFireAOEEffect()
    {
        // Instantiate the Fire AOE effect at the player's position
        GameObject fireAOE = Instantiate(fireAOEPrefab, playerTransform.position, Quaternion.identity);

        // Optionally, destroy the effect after it plays (e.g., 3 seconds)
        Destroy(fireAOE, 3f);
    }

    // This is an optional visual to show the blast radius in the editor
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the AOE max radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxBlastRadius);
    }
}
