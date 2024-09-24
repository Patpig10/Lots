using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AItest : MonoBehaviour
{
    public Transform[] blocks;            // Array of block transforms representing grid cells
    public float movementSpeed = 3f;      // Movement speed
    public float pauseDuration = 1f;      // Duration to pause at each block
    public float changeTargetInterval = 5f; // Time interval to change target

    private Transform targetBlock;        // The target block to move towards
    private bool isMoving = false;        // Check if movement is ongoing
    private Queue<Transform> recentBlocks = new Queue<Transform>(); // Queue to track the last two visited blocks

    void Start()
    {
      
        StartCoroutine(ChangeTargetRoutine());
    }

    void Update()
    {
        // Ensure the AI starts moving towards the target block if it's not already moving
        if (targetBlock != null && !isMoving)
        {
            StartCoroutine(MoveToBlock());
        }
    }

    // Coroutine to choose a new target block at regular intervals
    private IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            FindNearestBlock();
            yield return new WaitForSeconds(changeTargetInterval);  // Wait before choosing a new target
        }
    }

    // Method to find the nearest block that hasn't been visited recently
    private void FindNearestBlock()
    {
        if (blocks.Length > 0)
        {
            float minDistance = float.MaxValue;
            Transform nearestBlock = null;

            foreach (Transform block in blocks)
            {
                if (!recentBlocks.Contains(block))  // Check if the block hasn't been visited recently
                {
                    float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(block.position.x, 0, block.position.z));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestBlock = block;
                    }
                }
            }

            if (nearestBlock != null && nearestBlock != targetBlock)
            {
                // Update target block and manage the recent blocks queue
                if (recentBlocks.Count == 2)
                {
                    recentBlocks.Dequeue();  // Remove the oldest block from the queue
                }
                recentBlocks.Enqueue(nearestBlock); // Add the new block to the queue
                targetBlock = nearestBlock;
                Debug.Log("New target block set: " + targetBlock.name); // Debugging line
            }
        }
    }

    // Coroutine for smooth movement to the target block with 90-degree rotation
    private IEnumerator MoveToBlock()
    {
        isMoving = true;  // Set the flag to prevent movement while transitioning
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetBlock.position;
        targetPosition.y = startPosition.y;  // Ensure y remains constant

        // Determine the angle for 90-degree turns
        Vector3 directionToTarget = (targetPosition - startPosition).normalized;
        float angle = 0f;

        // Calculate the angle for movement direction (forward, backward, left, right)
        if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.z))
        {
            // Moving along the X axis (right/left)
            angle = directionToTarget.x > 0 ? 90f : -90f;  // Right or Left
        }
        else
        {
            // Moving along the Z axis (forward/backward)
            angle = directionToTarget.z > 0 ? 0f : 180f;  // Forward or Backward
        }

        // Apply the 90-degree rotation
        transform.rotation = Quaternion.Euler(0, angle, 0);

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        Debug.Log("Moving towards: " + targetBlock.name); // Debugging line

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;  // Wait for the next frame
        }

        transform.position = targetPosition;  // Snap to the final target position

        Debug.Log("Reached block: " + targetBlock.name); // Debugging line

        // Pause at the block
        yield return new WaitForSeconds(pauseDuration);

        isMoving = false;  // Allow new movements
    }
}
