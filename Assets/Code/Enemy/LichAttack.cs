using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichAttack : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to shoot
    public GameObject spikePrefab;      // The spike projectile for ground attack
    public Transform firePoint;          // The point where the projectile will be instantiated
    public float fireRate = 2f;          // Time between bursts
    public float detectionRange = 10f;   // Range in which the enemy can detect the player
    public Transform player;             // Reference to the player's transform
    public bool shootTowardPlayer = true; // Whether to shoot towards the player
    public int minShotsPerBurst = 3;     // Minimum number of shots per burst
    public int maxShotsPerBurst = 5;     // Maximum number of shots per burst
    public float timeBetweenShots = 0.2f; // Time between individual shots in a burst
    public float spreadAngle = 30f;      // Angle for the spread shot
    public int numberOfSpreadShots = 5;  // Number of shots in the spread attack
    public float spikeLifetime = 5f;     // Time before spikes are destroyed
    public int numberOfSpikes = 10;      // Number of spikes in the trail
    public float spikeSpacing = 1f;      // Distance between each spike in the trail
    public float attackCooldown = 3f;    // Delay between attacks

    private float fireTimer;
    private bool isShootingBurst = false; // Whether the enemy is currently shooting a burst
    private int shotsRemainingInBurst;   // Number of shots remaining in the current burst
    private int attackType = 0;          // 0 = basic shot, 1 = spread shot, 2 = ground spikes
    private bool isOnCooldown = false;   // Whether the enemy is currently on cooldown
    private LichRangerAI lichRangerAI;   // Reference to the LichRangerAI script
    public GameObject magic;             // Reference to the magic audio source
    public GameObject bones;             // Reference to the bones audio source
    public Animator animator;            // Reference to the animator

    void Start()
    {
        lichRangerAI = GetComponent<LichRangerAI>(); // Get the LichRangerAI component
    }

    void Update()
    {
        // Increment the timer each frame
        fireTimer += Time.deltaTime;

        // Only shoot if the player is in range, in front, the fireTimer has reached the fireRate, and not on cooldown
        if (PlayerInRange() && IsPlayerInFront() && fireTimer >= fireRate && !isShootingBurst && !isOnCooldown)
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
        // Stop the Lich's movement
        if (lichRangerAI != null)
        {
            // lichRangerAI.StopMovement();
        }

        // Determine the number of shots in this burst
        shotsRemainingInBurst = Random.Range(minShotsPerBurst, maxShotsPerBurst + 1);

        while (shotsRemainingInBurst > 0)
        {
            switch (attackType)
            {
                case 0:
                    yield return StartCoroutine(PlayAudioAndShoot(magic, () => ShootBasicProjectile()));
                    break;
                case 1:
                    yield return StartCoroutine(PlayAudioAndShoot(magic, () => ShootSpreadProjectile()));
                    break;
                case 2:
                    yield return StartCoroutine(PlayAudioAndShoot(bones, () => StartCoroutine(ShootGroundSpikeTrail())));
                    break;
            }
            shotsRemainingInBurst--;

            // Wait before shooting the next projectile in the burst
            yield return new WaitForSeconds(timeBetweenShots);
        }

        isShootingBurst = false;

        // Start cooldown after the burst is finished
        StartCoroutine(AttackCooldown());
    }

    // Coroutine to play audio and execute the attack after 0.8 seconds
    private IEnumerator PlayAudioAndShoot(GameObject audioSourceObject, System.Action attackAction)
    {
        // Play the audio
        audioSourceObject.GetComponent<AudioSource>().Play();

        // Wait for 0.8 seconds
        yield return new WaitForSeconds(0.8f);

        // Execute the attack
        attackAction();
    }

    // Shoot a single basic projectile
    private void ShootBasicProjectile()
    {
        animator.SetTrigger("Shot1");

        // Create the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Restrict the projectile to fire forward only
        projectile.transform.forward = transform.forward;  // Always shoot in the enemy's forward direction
    }

    // Shoot a spread of projectiles
    private void ShootSpreadProjectile()
    {
        animator.SetTrigger("Shot");

        float angleStep = spreadAngle / (numberOfSpreadShots - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < numberOfSpreadShots; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * firePoint.rotation;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
            projectile.transform.forward = rotation * Vector3.forward;
        }
    }

    // Shoot a trail of ground spikes toward the player
    private IEnumerator ShootGroundSpikeTrail()
    {
        animator.SetTrigger("Bones");

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Snap the direction to the nearest horizontal cardinal direction (forward, backward, left, or right)
        Vector3 cardinalDirection = GetNearestHorizontalCardinalDirection(directionToPlayer);

        for (int i = 0; i < numberOfSpikes; i++)
        {
            // Calculate the position for each spike
            Vector3 spikePosition = transform.position + cardinalDirection * (i * spikeSpacing);

            // Ensure correct rotation: -90 on X, align with movement direction
            Quaternion spikeRotation = Quaternion.LookRotation(cardinalDirection) * Quaternion.Euler(-90f, 0f, 0f);

            // Instantiate the spike at the calculated position
            GameObject spike = Instantiate(spikePrefab, spikePosition, spikeRotation);

            // Destroy the spike after a set time
            Destroy(spike, spikeLifetime);

            // Wait a short time before spawning the next spike
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Get the nearest horizontal cardinal direction (forward, backward, left, or right)
    private Vector3 GetNearestHorizontalCardinalDirection(Vector3 direction)
    {
        // Ignore the Y component to ensure only horizontal directions are considered
        direction.y = 0;
        direction.Normalize();

        // Determine the nearest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            // Left or right
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            // Forward or backward
            return direction.z > 0 ? Vector3.forward : Vector3.back;
        }
    }

    // Cooldown between attacks
    private IEnumerator AttackCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);

        // Resume the Lich's movement after cooldown
        if (lichRangerAI != null)
        {
            //lichRangerAI.ResumeMovement();
        }

        isOnCooldown = false;

        // Cycle through attack types after cooldown
        attackType = (attackType + 1) % 3;
    }
}