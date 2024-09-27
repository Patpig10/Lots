using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;       // Speed of the projectile
    public float lifetime = 5f;     // Time before the projectile is destroyed
    public bool destroyOnCollision = true; // Whether to destroy the projectile on collision

    void Start()
    {
        // Destroy the projectile after 'lifetime' seconds to prevent it from existing indefinitely
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward in the direction it's facing
        MoveForward();
    }

    // Move the projectile forward along its local forward axis
    private void MoveForward()
    {
        // Move the projectile along its forward direction at the defined speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // This method is called when the projectile collides with a non-trigger collider
    void OnCollisionEnter(Collision collision)
    {
        // Optionally, destroy the projectile upon collision with any object
        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }

        // Additional logic such as dealing damage can go here
    }

    // This method is called when the projectile enters a trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Optionally, destroy the projectile upon entering a trigger collider
        if (destroyOnCollision)
        {
            Destroy(gameObject);
        }

        // Additional logic such as dealing damage or triggering effects can go here
    }
}
