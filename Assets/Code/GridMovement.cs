using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float movementSpeed = 5f;      // Movement speed
    public float rotationSpeed = 180f;    // Speed of the rotation in degrees per second
    public Transform[] blocks;             // Array of block transforms representing grid cells
    public Animator animator;              // Animator reference

    private Transform targetBlock;         // The target block to move towards
    private bool isMoving = false;         // Check if movement is ongoing
    private Quaternion targetRotation;     // The target rotation to smoothly rotate towards
    public float searchRadius = 5f;        // Define the radius for searching blocks

    void Start()
    {
        // Populate the blocks array with all GrassBlock objects in the scene
        GrassBlock[] grassBlocks = FindObjectsOfType<GrassBlock>();
        blocks = new Transform[grassBlocks.Length];

        for (int i = 0; i < grassBlocks.Length; i++)
        {
            blocks[i] = grassBlocks[i].transform; // Add the transform of each GrassBlock to the blocks array
        }
    }

    void Update()
    {

        if (!isMoving)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 input = Vector3.zero;

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                input = new Vector3(horizontalInput, 0, 0);  // Prioritize horizontal movement
            }
            else if (Mathf.Abs(verticalInput) > 0)
            {
                input = new Vector3(0, 0, verticalInput);  // Prioritize vertical movement
            }

            if (input != Vector3.zero)
            {
                RotatePlayerSmooth(input);

                // Find the nearest block and check for unpassable blocks
                if (FindNearestBlock(input))
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("Move");
                    }

                    StartCoroutine(MoveToBlock());
                }
            }
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void RotatePlayerSmooth(Vector3 input)
    {
        float targetAngle = 0f;

        if (input.x > 0)       // Moving right
            targetAngle = 90f;
        else if (input.x < 0)  // Moving left
            targetAngle = -90f;
        else if (input.z > 0)  // Moving forward
            targetAngle = 0f;
        else if (input.z < 0)  // Moving backward
            targetAngle = 180f;

        targetRotation = Quaternion.Euler(0, targetAngle, 0);
    }

    public bool FindNearestBlock(Vector3 direction)
    {
        float minDistance = float.MaxValue;
        Transform nearestBlock = null;

        Vector3 normalizedDirection = direction.normalized;

        foreach (Transform block in blocks)
        {
            // Check if the block is still valid
            if (block == null)
            {
                continue; // Skip destroyed blocks
            }

            // Calculate the distance from the player to the block
            float distanceToBlock = Vector3.Distance(transform.position, block.position);

            // Only consider blocks within the search radius
            if (distanceToBlock <= searchRadius)
            {
                Vector3 directionToBlock = (block.position - transform.position).normalized;
                float dotProduct = Vector3.Dot(normalizedDirection, directionToBlock);

                // Only proceed if the block is within a certain angle (facing)
                if (dotProduct > 0.9f)
                {
                    // Update the nearest block if it's closer and valid
                    if (distanceToBlock < minDistance)
                    {
                        minDistance = distanceToBlock;
                        nearestBlock = block; // Store the nearest valid block
                    }
                }
            }
        }

        if (nearestBlock != null)
        {
            // Check if the nearest block is unpassable before setting it as the target
            if (nearestBlock.CompareTag("Unpassable"))
            {
                return false; // Block the movement if the target block is unpassable
            }

            targetBlock = nearestBlock; // Set the target block to move towards
            return true; // Found a valid target block to move towards
        }

        return false; // No valid block found
    }

    public IEnumerator MoveToBlock()
    {
        isMoving = true;

        if (targetBlock == null)
        {
            Debug.LogWarning("Target block is null, cannot move.");
            isMoving = false;
            yield break; // Exit if there's no target block
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetBlock.position.x, startPosition.y, targetBlock.position.z);
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPosition;

        // Check if the target block has a portal
        Portal portal = targetBlock.GetComponent<Portal>();
        if (portal != null)
        {
            portal.TeleportPlayer();  // Teleport the player using the portal
        }

        isMoving = false;
        Debug.Log("Movement completed to block: " + targetBlock.name);
    
}

    public void Knockback(int knockbackDistance, Vector3 knockbackDirection)
    {
        StartCoroutine(ApplyKnockback(knockbackDistance, knockbackDirection));
    }

    private IEnumerator ApplyKnockback(int knockbackDistance, Vector3 knockbackDirection)
    {
        isMoving = true;  // Prevent other inputs during knockback

        for (int i = 0; i < knockbackDistance; i++)
        {
            if (FindNearestBlock(knockbackDirection))
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = new Vector3(targetBlock.position.x, startPosition.y, targetBlock.position.z);

                float journeyLength = Vector3.Distance(startPosition, targetPosition);
                float startTime = Time.time;

                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    float distanceCovered = (Time.time - startTime) * movementSpeed;
                    float fractionOfJourney = distanceCovered / journeyLength;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                    yield return null;
                }

                transform.position = targetPosition;
            }
            else
            {
                // If no valid block is found, stop the knockback process
                break;
            }
        }

        isMoving = false;  // Re-enable movement after knockback is complete
    }

    public IEnumerator MoveToBlockWithStrength(float knockbackSpeed)
    {
        isMoving = true;  // Set the flag to prevent movement while transitioning

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(targetBlock.position.x, startPosition.y, targetBlock.position.z); // Keep Y position unchanged
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * knockbackSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;  // Wait for the next frame
        }

        transform.position = targetPosition;  // Snap to the final target position

        isMoving = false;  // Allow new movements
    }
}