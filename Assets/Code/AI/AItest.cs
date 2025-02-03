using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AItest : MonoBehaviour
{
    public Transform[] blocks;            // Array of block transforms representing grid cells
    public Transform player;              // Reference to the player's transform
    public float movementSpeed = 3f;      // Movement speed
    public float pauseDuration = 1f;      // Duration to pause at each block
    public float detectionRadius = 10f;   // Radius to detect the player

    private Transform targetBlock;        // The next block to move towards
    private bool isMoving = false;        // Check if movement is ongoing
    private Queue<Transform> recentBlocks = new Queue<Transform>(); // Queue to track the last two visited blocks

    void Start()
    {
        StartCoroutine(ChangeTargetRoutine());
    }

    void Update()
    {
        if (!isMoving)
        {
            if (IsPlayerNearby()) // If the player is nearby, calculate the path to them
            {
                FindNextBlockTowardsPlayer();
            }

            if (targetBlock != null) // Move to the calculated target block
            {
                StartCoroutine(MoveToBlock());
            }
        }
    }

    // Coroutine to choose a new block at regular intervals when the player is not nearby
    private IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            if (!IsPlayerNearby()) // Only find a new block if the player isn't nearby
            {
                FindNearestBlock();
            }
            yield return new WaitForSeconds(1f); // Shorter interval for smoother behavior
        }
    }

    // Method to detect if the player is within the detection radius
    private bool IsPlayerNearby()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRadius;
    }

    // Method to find the next block that brings the AI closer to the player
    private void FindNextBlockTowardsPlayer()
    {
        Transform nearestBlock = null;
        float minDistanceToPlayer = float.MaxValue;

        foreach (Transform block in blocks)
        {
            // Skip blocks that are unpassable, recently visited, or occupied
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block))
            {
                continue;
            }

            // Check if the block is horizontally or vertically aligned with the AI's position
            if (Mathf.Abs(block.position.x - transform.position.x) < 0.1f ||
                Mathf.Abs(block.position.z - transform.position.z) < 0.1f)
            {
                // Calculate the Manhattan distance to the player
                float distanceToPlayer = Mathf.Abs(block.position.x - player.position.x) +
                                         Mathf.Abs(block.position.z - player.position.z);

                if (distanceToPlayer < minDistanceToPlayer)
                {
                    minDistanceToPlayer = distanceToPlayer;
                    nearestBlock = block;
                }
            }
        }

        if (nearestBlock != null)
        {
            if (recentBlocks.Count == 2)
            {
                recentBlocks.Dequeue(); // Remove the oldest block
            }
            recentBlocks.Enqueue(nearestBlock); // Add the new block to the queue
            targetBlock = nearestBlock;
            Debug.Log("Moving closer to the player via block: " + targetBlock.name);
        }
    }

    // Method to find the nearest block when the player is not nearby
    private void FindNearestBlock()
    {
        if (blocks.Length > 0)
        {
            float minDistance = float.MaxValue;
            Transform nearestBlock = null;

            foreach (Transform block in blocks)
            {
                // Skip blocks that are unpassable, recently visited, or occupied
                if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block))
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, block.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBlock = block;
                }
            }

            if (nearestBlock != null)
            {
                if (recentBlocks.Count == 2)
                {
                    recentBlocks.Dequeue();
                }
                recentBlocks.Enqueue(nearestBlock);
                targetBlock = nearestBlock;
                Debug.Log("New target block set: " + targetBlock.name);
            }
        }
    }

    // Coroutine for smooth movement to the target block
    private IEnumerator MoveToBlock()
    {
        isMoving = true;  // Set the flag to prevent movement while transitioning
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetBlock.position;
        targetPosition.y = startPosition.y;  // Ensure y remains constant

        // Determine the rotation for straight movement
        Vector3 directionToTarget = (targetPosition - startPosition).normalized;
        float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        Debug.Log("Moving towards: " + targetBlock.name);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;  // Wait for the next frame
        }

        transform.position = targetPosition;  // Snap to the final target position

        Debug.Log("Reached block: " + targetBlock.name);

        // Pause at the block
        yield return new WaitForSeconds(pauseDuration);

        isMoving = false;  // Allow new movements
    }

    // Method to check if a block is occupied by an object tagged as "Enemy"
    private bool IsBlockOccupied(Transform block)
    {
        Collider[] colliders = Physics.OverlapSphere(block.position, 0.1f); // Check for objects in the block's position
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Unpassable")) // If an object tagged "Enemy" or "Unpassable" is found, the block is occupied
            {
                return true;
            }
        }
        return false;
    }
}