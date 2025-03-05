using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float expansionSpeed = 5f;  // Speed at which the shockwave expands
    public float maxSize = 10f;        // Maximum size of the shockwave
    public int damage = 1;             // Damage dealt to the player per tick
    public float damageInterval = 1f;  // Time between damage ticks (in seconds)
    public LayerMask playerLayer;      // Layer for the player
    public LayerMask obstacleLayer;    // Layer for unpassable objects

    private HashSet<Collider> playersInShockwave = new HashSet<Collider>(); // Track players inside the shockwave

    private void Update()
    {
        // Expand the shockwave
        transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

        // Check for collisions with the player
        Collider[] hits = Physics.OverlapSphere(transform.position, transform.localScale.x / 2f, playerLayer);

        foreach (Collider hit in hits)
        {
            // Check if the player is not already in the shockwave
            if (!playersInShockwave.Contains(hit))
            {
                // Check if there is a clear line of sight to the player
                if (HasLineOfSight(hit.transform))
                {
                    // Add the player to the shockwave and start damaging them
                    playersInShockwave.Add(hit);
                    StartCoroutine(DamagePlayerOverTime(hit));
                }
            }
        }

        // Destroy the shockwave when it reaches max size
        if (transform.localScale.x >= maxSize)
        {
            Destroy(gameObject);
        }
    }

    private bool HasLineOfSight(Transform target)
    {
        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        // Perform a raycast to check for obstacles with the "unpassable" tag
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance, obstacleLayer))
        {
            // If the raycast hits something, check if it has the "unpassable" tag
            if (hit.collider.CompareTag("unpassable"))
            {
                // There is an unpassable object blocking the line of sight
                return false;
            }
        }

        // No unpassable object is blocking the line of sight
        return true;
    }

    private IEnumerator DamagePlayerOverTime(Collider playerCollider)
    {
        while (playersInShockwave.Contains(playerCollider))
        {
            // Check if the player is still in the shockwave and has line of sight
            if (playerCollider != null && HasLineOfSight(playerCollider.transform))
            {
                // Apply damage to the player
                HeartSystem heartSystem = playerCollider.GetComponent<HeartSystem>();
                if (heartSystem != null)
                {
                    heartSystem.TakeDamage(damage);
                }
            }

            // Wait for the damage interval
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the player from the shockwave when they exit
        if (other.CompareTag("Player"))
        {
            playersInShockwave.Remove(other);
        }
    }
}