using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to shoot
    public Transform firePoint;          // The point where the projectile will be instantiated
    public float fireRate = 2f;          // Time between shots
    public float detectionRange = 10f;   // Range in which the enemy can detect the player
    public Transform player;             // Reference to the player's transform
    public bool shootTowardPlayer = true; // Whether to shoot towards the player

    private float fireTimer;

    void Update()
    {
        // Increment the timer each frame
        fireTimer += Time.deltaTime;

        // Detect player and shoot if within range
        if (PlayerInRange() && fireTimer >= fireRate && IsPlayerInFront())
        {
            ShootProjectile();
            fireTimer = 0f;  // Reset the timer after shooting
        }
    }

    // Check if the player is within the detection range
    private bool PlayerInRange()
    {
        if (player == null)
            return false;

        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRange;
    }

    // Check if the player is in front of the enemy within a certain angle range
    private bool IsPlayerInFront()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // Only allow shooting if the player is within a 45-degree arc in front of the enemy
        return dotProduct > 0.7f;  // Adjust this value to narrow or widen the angle
    }

    // Shoot a projectile
    private void ShootProjectile()
    {
        // Create the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Optionally, make the projectile face the player before firing
        if (shootTowardPlayer && player != null)
        {
            Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
            projectile.transform.forward = directionToPlayer;  // Rotate the projectile to face the player
        }
    }
}
