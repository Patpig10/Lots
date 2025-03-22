using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to shoot
    public Transform firePoint;  // The point where the projectile will be spawned
    public float projectileSpeed = 10f;  // Speed of the projectile
    private HeartSystem heartSystem;  // Reference to the HeartSystem
    public GameObject slimeshot;
    public Animator Player;
    public float shootDelay = 1f;  // Delay before shooting the projectile

    void Start()
    {
        // Find the HeartSystem component in the scene
        heartSystem = FindObjectOfType<HeartSystem>();

        if (heartSystem == null)
        {
            Debug.LogError("HeartSystem not found in the scene!");
        }
    }

    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootProjectile();
        }*/
    }

    public void ShootProjectile()
    {
        // Trigger the shoot animation
        Player.SetTrigger("Shoot");

        // Play the shooting sound

        // Start the coroutine to delay the projectile instantiation
        StartCoroutine(ShootAfterDelay());
    }

    private IEnumerator ShootAfterDelay()
    {
        slimeshot.GetComponent<AudioSource>().Play();

        // Wait for the specified delay
        yield return new WaitForSeconds(shootDelay);

        // Check if the player has enough hearts to shoot
        if (heartSystem != null && heartSystem.GetCurrentLife() > 0)
        {
            // Spawn the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Give the projectile velocity in the direction the player is facing
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }

            // Remove one heart
            // heartSystem.TakeDamage(1);
        }
        else
        {
            Debug.Log("Not enough hearts to shoot!");
        }
    }
}