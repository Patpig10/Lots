using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter2 : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to shoot
    public Transform firePoint;          // The point where the projectile will be instantiated
    public float fireRate = 2f;          // Time between bursts
    public float detectionRange = 10f;   // Range in which the enemy can detect the player
    public Transform player;             // Reference to the player's transform
    public bool shootTowardPlayer = true; // Whether to shoot towards the player
    public int minShotsPerBurst = 3;     // Minimum number of shots per burst
    public int maxShotsPerBurst = 5;     // Maximum number of shots per burst
    public float timeBetweenShots = 0.2f; // Time between individual shots in a burst

    private float fireTimer;
    private bool isShootingBurst = false; // Whether the enemy is currently shooting a burst
    private int shotsRemainingInBurst;   // Number of shots remaining in the current burst

    void Update()
    {
        // Increment the timer each frame
        fireTimer += Time.deltaTime;

        // Only shoot if the player is in range, in front, and the fireTimer has reached the fireRate
        if (PlayerInRange() && IsPlayerInFront() && fireTimer >= fireRate && !isShootingBurst)
        {
            StartCoroutine(ShootBurst());
            fireTimer = 0f;  // Reset the timer after starting a burst
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

    // Check if the player is in front of the enemy within a narrow forward range
    private bool IsPlayerInFront()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // Only allow shooting if the player is directly in front within a narrow angle
        return dotProduct > 0.95f;  // Increase the threshold to make the forward range narrower
    }

    // Shoot a burst of projectiles
    private IEnumerator ShootBurst()
    {
        isShootingBurst = true;

        // Determine the number of shots in this burst
        shotsRemainingInBurst = Random.Range(minShotsPerBurst, maxShotsPerBurst + 1);

        while (shotsRemainingInBurst > 0)
        {
            ShootProjectile();
            shotsRemainingInBurst--;

            // Wait before shooting the next projectile in the burst
            yield return new WaitForSeconds(timeBetweenShots);
        }

        isShootingBurst = false;
    }

    // Shoot a single projectile
    private void ShootProjectile()
    {
        // Create the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Restrict the projectile to fire forward only
        projectile.transform.forward = transform.forward;  // Always shoot in the enemy's forward direction
    }
}
