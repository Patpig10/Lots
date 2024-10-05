using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnakeAI : MonoBehaviour
{
    public Transform[] blocks;            // Array of block transforms representing grid cells
    public GameObject bodyPrefab;         // The body segment prefab
    public float movementSpeed = 3f;      // Movement speed
    public float pauseDuration = 1f;      // Duration to pause at each block
    public float changeTargetInterval = 5f; // Time interval to change target
    public float bodyGrowthInterval = 2f;  // Time interval to spawn new body segment
    public int initialBodySize = 3;       // Initial size of the boss (number of body segments)
    public float blockCooldownDuration = 30f; // Cooldown duration for blocks

    private Transform targetBlock;        // The target block to move towards
    private bool isMoving = false;        // Check if movement is ongoing
    private Dictionary<Transform, float> blockCooldowns = new Dictionary<Transform, float>(); // Dictionary to track block cooldowns
    private List<Transform> bodyParts = new List<Transform>();      // List of body segments
    private List<Vector3> previousPositions = new List<Vector3>();  // List of previous positions on the grid

    private float growthTimer = 0f;       // Timer for body growth

    void Start()
    {
        StartCoroutine(ChangeTargetRoutine());

        // Create the initial body segments
        for (int i = 0; i < initialBodySize; i++)
        {
            AddBodySegment(true);  // Initialize with proper spacing
        }
    }

    void Update()
    {
        // Ensure the AI starts moving towards the target block if it's not already moving
        if (targetBlock != null && !isMoving)
        {
            StartCoroutine(MoveToBlock());
        }

        // Update the growth timer and grow the body when the interval is reached
        growthTimer += Time.deltaTime;
        if (growthTimer >= bodyGrowthInterval)
        {
            growthTimer = 0f;
            AddBodySegment(false); // Grow the body
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

    // Method to find the nearest block that hasn't been visited recently and is not occupied by the body
    private void FindNearestBlock()
    {
        if (blocks.Length > 0)
        {
            float minDistance = float.MaxValue;
            Transform nearestBlock = null;

            foreach (Transform block in blocks)
            {
                // Check if the block is not on cooldown and not occupied
                if (!IsBlockOnCooldown(block) && !IsBlockOccupied(block))
                {
                    Debug.Log("Considering block: " + block.name); // Debugging line
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
                // Update target block and set the cooldown
                targetBlock = nearestBlock;
                blockCooldowns[targetBlock] = Time.time + blockCooldownDuration; // Set cooldown
                Debug.Log("New target block set: " + targetBlock.name); // Debugging line
            }
            else
            {
                Debug.Log("No valid target block found."); // Debugging line
            }
        }
    }

    // Check if a block is on cooldown
    private bool IsBlockOnCooldown(Transform block)
    {
        if (blockCooldowns.TryGetValue(block, out float cooldownEndTime))
        {
            return Time.time < cooldownEndTime; // Check if the current time is less than the cooldown end time
        }
        return false; // Block not found in cooldowns, meaning it can be used
    }

    // Method to check if a block is occupied by any body segment
    private bool IsBlockOccupied(Transform block)
    {
        // Use a small tolerance to account for floating-point discrepancies
        float tolerance = 0.01f;

        foreach (Transform bodyPart in bodyParts)
        {
            // Check if the body part occupies the same position as the block, with a tolerance
            if (Vector3.Distance(bodyPart.position, block.position) < tolerance)
            {
                return true; // The block is occupied
            }
        }
        return false; // The block is not occupied
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
        previousPositions.Insert(0, targetPosition);  // Store the new position in the list of previous positions

        // Keep the list of previous positions limited to the number of body parts + 1
        if (previousPositions.Count > bodyParts.Count + 1)
        {
            previousPositions.RemoveAt(previousPositions.Count - 1);
        }

        // Update the body segments after the boss has moved to the new block
        UpdateBodyMovement();

        Debug.Log("Reached block: " + targetBlock.name); // Debugging line

        // Pause at the block
        yield return new WaitForSeconds(pauseDuration);

        isMoving = false;  // Allow new movements
    }

    // Method to add a body segment behind the last one
    private void AddBodySegment(bool isInitialSetup)
    {
        Vector3 newBodyPosition;

        if (bodyParts.Count == 0)
        {
            // Place the first body segment directly behind the head
            newBodyPosition = transform.position - transform.forward;  // Initially place it 1 unit behind
        }
        else
        {
            // Place subsequent body segments behind the last one
            newBodyPosition = bodyParts[bodyParts.Count - 1].position - (bodyParts[bodyParts.Count - 1].forward); // Ensure it is 1 unit behind the last body part
        }

        // Instantiate the new body part with a rotation of x = -90 degrees
        Transform newBodyPart = Instantiate(bodyPrefab, newBodyPosition, Quaternion.Euler(-90f, 0f, 0f)).transform;
        bodyParts.Add(newBodyPart);

        // Add the initial setup position to the list
        if (isInitialSetup)
        {
            previousPositions.Add(newBodyPart.position);
        }
    }

    // Method to update the movement of body segments to follow the head on a grid
    private void UpdateBodyMovement()
    { // Ensure there are enough previous positions to move the body parts
      // Ensure there are enough previous positions to move the body parts
        if (previousPositions.Count < bodyParts.Count + 1) return;

        // Start from the last body segment and move each to the position of the segment ahead of it
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            // The first body segment follows the position the head was previously at
            Vector3 targetPosition = previousPositions[i + 1];

            // Calculate the direction for rotation
            Vector3 direction = targetPosition - bodyParts[i].position;

            // Move the body part to the new position
            bodyParts[i].position = targetPosition;

            // Rotate the body part to face the correct direction
            if (direction != Vector3.zero)
            {
                float angle = 0f;

                // Determine the angle based on the direction of movement (right, left, up, or down)
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Moving along the X axis (right/left)
                    angle = direction.x > 0 ? -90f : 90f;  // Right or Left
                }
                else
                {
                    // Moving along the Y axis (up/down)
                    angle = direction.y > 0 ? 0f : 180f;  // Up or Down
                }

                // Apply the rotation on the Z axis with a fixed -90 on the X axis
                bodyParts[i].rotation = Quaternion.Euler(-90f, 0f, angle);
            }
        }

        // After all body parts have moved, insert the head's position at the beginning of the previous positions list
        previousPositions.Insert(0, transform.position);

        // Keep the previous positions list to the size of the body parts + 1 (for the head)
        if (previousPositions.Count > bodyParts.Count + 1)
        {
            previousPositions.RemoveAt(previousPositions.Count - 1);
        }
    }
}
