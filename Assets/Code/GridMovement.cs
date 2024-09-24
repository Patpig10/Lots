using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float movementSpeed = 5f;      // Movement speed
    public float rotationSpeed = 180f;    // Speed of the rotation in degrees per second
    public Transform[] blocks;            // Array of block transforms representing grid cells
    public Animator animator;             // Animator reference

    private Transform targetBlock;        // The target block to move towards
    private bool isMoving = false;        // Check if movement is ongoing
    private Quaternion targetRotation;    // The target rotation to smoothly rotate towards

    void Update()
    {
        // Only process input if the player isn't currently moving
        if (!isMoving)
        {
            // Get player input for horizontal (X axis) and vertical (Z axis) movement
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (input != Vector3.zero)
            {
                // Rotate player to face the correct direction (90-degree increments)
                RotatePlayerSmooth(input);

                // Check if the nearest block is passable before moving
                if (FindNearestBlock(input)) // Find nearest block returns true if a valid block is found
                {
                    // Trigger the "Move" animation immediately when movement starts
                    if (animator != null)
                    {
                        animator.SetTrigger("Move");
                    }

                    // Start smooth movement to the nearest block
                    StartCoroutine(MoveToBlock());
                }
            }
        }

        // Smoothly update the rotation each frame
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Smoothly rotate the player based on the movement direction
    private void RotatePlayerSmooth(Vector3 input)
    {
        float targetAngle = 0f;

        // Determine the target rotation angle based on the input direction
        if (input.x > 0)       // Moving right
            targetAngle = 90f;
        else if (input.x < 0)  // Moving left
            targetAngle = -90f;
        else if (input.z > 0)  // Moving forward
            targetAngle = 0f;
        else if (input.z < 0)  // Moving backward
            targetAngle = 180f;

        // Set the target rotation to smoothly rotate towards
        targetRotation = Quaternion.Euler(0, targetAngle, 0);
    }

    // Find the nearest block in the direction of input
    // Return true if a valid block is found, otherwise false

    private bool FindNearestBlock(Vector3 direction)
    {
        float minDistance = float.MaxValue;
        Transform nearestBlock = null;

        // Normalize the direction once to avoid recalculating
        Vector3 normalizedDirection = direction.normalized;

        foreach (Transform block in blocks)
        {
            // Calculate the vector from player to block and normalize
            Vector3 directionToBlock = (block.position - transform.position).normalized;

            // Calculate dot product to check if the block is in the same direction as input
            float dotProduct = Vector3.Dot(normalizedDirection, directionToBlock);

            // Only consider blocks in the direction of movement
            if (dotProduct > 0.9f)
            {
                // Calculate distance to the block
                float distance = Vector3.Distance(transform.position, block.position);

                // If the block is unpassable and it's closer than other blocks, stop movement
                if (block.CompareTag("Unpassable"))
                {
                    // If an unpassable block is in the direction and closer than others, stop movement
                    if (distance < minDistance)
                    {
                        return false;  // Immediately stop if unpassable block is closest
                    }
                }

                // If this block is passable and closer than previously found blocks, select it
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBlock = block;
                }
            }
        }

        // Set the target block if one was found
        if (nearestBlock != null)
        {
            targetBlock = nearestBlock;
            return true;
        }

        // No valid block found in the direction
        return false;
    }


    // Coroutine for smooth movement to the target block
    private IEnumerator MoveToBlock()
    {
        isMoving = true;  // Set the flag to prevent movement while transitioning

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetBlock.position.x, startPosition.y, targetBlock.position.z); // Keep Y position unchanged
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;  // Wait for the next frame
        }

        transform.position = targetPosition;  // Snap to the final target position

        isMoving = false;  // Allow new movements
    }
    public void Knockback()
    {
        // Define the knockback direction (down on the grid, which is negative Z)
        Vector3 knockbackDirection = new Vector3(0, 0, -1);

        // Check if the nearest block in the knockback direction is passable
        if (FindNearestBlock(knockbackDirection))
        {
            // Start movement to the block one grid down
            StartCoroutine(MoveToBlock());
        }
    }
}


