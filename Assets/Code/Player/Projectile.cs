using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;                // Speed of the projectile
    public float lifetime = 5f;              // Time before the projectile is destroyed
    public bool destroyOnCollision = true;   // Whether to destroy the projectile on collision
    public bool destroyOnCollisionPlayer = true; // Whether to destroy the projectile on collision with the player
    public int knockbackDistance = 2;        // The number of grid cells to knock back the player
    public float knockbackStrength = 5f;     // The strength of the knockback (speed of knockback movement)
    public bool playerProjectile = false;    // Whether the projectile is a player projectile
    public Weapon weapon;                    // Reference to the Weapon script

    private void Start()
    {
        // Destroy the projectile after 'lifetime' seconds to prevent it from existing indefinitely
        Destroy(gameObject, lifetime);

        // Find the Weapon script only once at the start
        if (weapon == null)
        {
            weapon = GameObject.Find("Layer.3")?.GetComponent<Weapon>();
        }
    }

    private void Update()
    {
        // Move the projectile forward in the direction it's facing
        MoveForward();
    }

    // Move the projectile forward along its local forward axis
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Handle trigger collisions (e.g., hitting the player, enemies, or other objects)
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    // Handle regular collisions (e.g., hitting walls or other objects)
    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    // Centralized method to handle collisions
    private void HandleCollision(GameObject other)
    {
        // Handle collision with unpassable objects (e.g., walls)
        if (other.CompareTag("Unpassable"))
        {
            Destroy(gameObject);
            return;
        }
        if (other.CompareTag("Block"))
        {
            Destroy(gameObject);
            return;
        }
        // Handle collision with the player
        if (other.CompareTag("Player"))
        {
            if (destroyOnCollisionPlayer)
            {
                Destroy(gameObject, 0.1f);
            }
            return;
        }

        // Handle collision with enemies
        if (other.CompareTag("Enemy"))
        {
            if (destroyOnCollision)
            {
                Destroy(gameObject, 0.1f);
            }

            // Apply knockback to the enemy (if applicable)
            PlayerKnockbackTest enemyKnockback = other.GetComponent<PlayerKnockbackTest>();
            if (enemyKnockback != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                StartCoroutine(enemyKnockback.ApplyKnockback(knockbackDirection, knockbackDistance, knockbackStrength));
            }
            return;
        }

        // Handle collision with the Orb (or other damageable objects)
        OrbHealth orb = other.GetComponent<OrbHealth>();
        if (orb != null)
        {
            orb.TakeDamage(weapon != null ? weapon.damage : 1); // Deal damage to the orb
            Destroy(gameObject, 0.1f); // Destroy the projectile after dealing damage
            return;
        }

        // Destroy the projectile on collision with any other object (if enabled)
        if (destroyOnCollision)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}