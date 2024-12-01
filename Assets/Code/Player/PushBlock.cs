using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour
{
    public float pushForce = 5f;

    private void OnCollisionStay(Collision collision)
    {
        // Check if the collided object is pushable
        if (collision.gameObject.CompareTag("Pushable"))
        {
            // Get movement direction from player input
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            // Apply force to the pushable block
            if (direction.magnitude > 0.1f)
            {
                Rigidbody blockRb = collision.gameObject.GetComponent<Rigidbody>();
                if (blockRb != null)
                {
                    blockRb.MovePosition(blockRb.position + direction * pushForce * Time.deltaTime);
                }
            }
        }
    }
}
