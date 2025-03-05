using System.Collections;
using UnityEngine;

public class BossHeadController : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of the head's left-right movement
    public float moveDistance = 5f; // Distance the head moves left and right
    public float shootCooldown = 5f; // Cooldown between shooting actions
    public float moveDownDuration = 2f; // Duration the jaw stays down before shooting
    public GameObject projectilePrefab; // Prefab for the projectile to shoot
    public Transform shootPoint; // Position where the projectile is spawned
    public GameObject jaw; // Reference to the jaw GameObject
    public int minShots = 1; // Minimum number of shots per cycle
    public int maxShots = 3; // Maximum number of shots per cycle
    public float jawDropDistance = 5.85f; // Distance the jaw moves along the Z-axis (from z = 0 to z = -5.85)

    private Vector3 headStartPosition; // Starting position of the head
    private Vector3 jawStartPosition; // Starting position of the jaw
    private bool isShooting = false; // Track if the head is shooting

    private void Start()
    {
        headStartPosition = transform.position; // Store the starting position of the head
        jawStartPosition = jaw.transform.localPosition; // Store the starting local position of the jaw
        StartCoroutine(HeadMovement()); // Start the head movement coroutine
        StartCoroutine(ShootRoutine()); // Start the shooting coroutine
    }

    private IEnumerator HeadMovement()
    {
        while (true)
        {
            if (!isShooting) // Only move left/right if not shooting
            {
                // Move the head to the right
                transform.position = headStartPosition + Vector3.right * moveDistance;
                yield return new WaitForSeconds(1f / moveSpeed); // Wait for the movement delay

                // Move the head back to the left
                transform.position = headStartPosition;
                yield return new WaitForSeconds(1f / moveSpeed); // Wait for the movement delay
            }
            else
            {
                yield return null; // Wait for the next frame if shooting
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootCooldown); // Wait for the cooldown

            if (!isShooting) // Only shoot if not already shooting
            {
                StartCoroutine(MoveDownAndShoot());
            }
        }
    }

    private IEnumerator MoveDownAndShoot()
    {
        isShooting = true;

        // Move the jaw along the Z-axis from z = 0 to z = -5.85 (blocky movement)
        jaw.transform.localPosition = jawStartPosition + Vector3.forward * -jawDropDistance;
        yield return new WaitForSeconds(0.1f); // Short delay for blocky effect

        // Shoot 1-3 projectiles
        int shots = Random.Range(minShots, maxShots + 1);
        for (int i = 0; i < shots; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(0.5f); // Delay between shots
        }

        // Wait for a moment before moving the jaw back up
        yield return new WaitForSeconds(moveDownDuration);

        // Move the jaw back along the Z-axis from z = -5.85 to z = 0 (blocky movement)
        jaw.transform.localPosition = jawStartPosition;
        yield return new WaitForSeconds(0.1f); // Short delay for blocky effect

        isShooting = false;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            // Spawn the projectile with the correct rotation (facing forward)
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Debug.Log("Boss head shot a projectile!");
        }
    }
}