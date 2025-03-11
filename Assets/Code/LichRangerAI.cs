using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichRangerAI : MonoBehaviour
{
    public Transform[] blocks;            // Array of block transforms representing grid cells
    public Transform player;              // Reference to the player's transform
    public float movementSpeed = 3f;      // Base movement speed
    public float increasedSpeed = 6f;     // Increased speed when player is nearby
    public float pauseDuration = 1f;      // Duration to pause at each block
    public float detectionRadius = 10f;   // Radius to detect the player
    public float runAwayRadius = 5f;      // Radius to trigger running away from the player
    public float maxRunAwayDistance = 10f; // Maximum distance the boss can run away
    public Transform[] guidedSpots;       // Array of predefined spots the boss can be guided to
    public float guidedSpotChance = 0.2f; // Chance to move towards a guided spot instead of away from the player
    public float chargeChance = 0.1f;     // Chance to charge towards the player when nearby
    public float runAwayChance = 0.3f;    // Chance to run away from the player when nearby
    public float rotationSpeed = 5f;      // Speed at which the boss rotates to face the player
    public GameObject projectilePrefab;   // Prefab for the ranged attack projectile
    public float attackCooldown = 2f;     // Cooldown between ranged attacks
    public float attackRange = 7f;        // Range at which the Lich Ranger will attack

    private Transform targetBlock;        // The next block to move towards
    private bool isMoving = false;        // Check if movement is ongoing
    private Queue<Transform> recentBlocks = new Queue<Transform>(); // Queue to track the last two visited blocks
    private bool isAttacking = false;     // Check if the Lich Ranger is attacking
    private float lastAttackTime = 0f;    // Time of the last attack

    void Start()
    {
        StartCoroutine(ChangeTargetRoutine());
    }

    private void OnEnable()
    {
        isMoving = false;
        targetBlock = null;
        StartCoroutine(ChangeTargetRoutine());
    }

    void Update()
    {
        if (!isMoving && !isAttacking)
        {
            if (IsPlayerNearby()) // If the player is nearby, calculate the path to move away, to a guided spot, or charge
            {
                if (ShouldRunAway() && Random.value < runAwayChance)
                {
                    FindNextBlockAwayFromPlayer();
                }
                else if (Random.value < chargeChance)
                {
                    ChargeTowardsPlayer();
                }
                else if (Random.value < guidedSpotChance)
                {
                    MoveToGuidedSpot();
                }
                else
                {
                    FindNextBlockTowardsPlayer();
                }
            }

            if (targetBlock != null) // Move to the calculated target block
            {
                StartCoroutine(MoveToBlock());
            }
        }

        // Always face the player while in range
        if (IsPlayerNearby())
        {
            FacePlayer();
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

    // Method to check if the player is within the run-away radius
    private bool ShouldRunAway()
    {
        return Vector3.Distance(transform.position, player.position) <= runAwayRadius;
    }

    // Method to find the next block that moves the AI away from the player
    private void FindNextBlockAwayFromPlayer()
    {
        Transform farthestBlock = null;
        float maxDistanceToPlayer = float.MinValue;

        foreach (Transform block in blocks)
        {
            // Skip blocks that are unpassable, recently visited, occupied, or occupied by the player
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block) || IsBlockOccupiedByPlayer(block))
            {
                continue;
            }

            // Check if the block is within the maxRunAwayDistance from the boss's current position
            float distanceFromBoss = Vector3.Distance(block.position, transform.position);
            if (distanceFromBoss > maxRunAwayDistance)
            {
                continue; // Skip blocks that are too far away
            }

            // Check if the block is horizontally or vertically aligned with the AI's position
            if (Mathf.Abs(block.position.x - transform.position.x) < 0.1f ||
                Mathf.Abs(block.position.z - transform.position.z) < 0.1f)
            {
                // Calculate the Manhattan distance to the player
                float distanceToPlayer = Mathf.Abs(block.position.x - player.position.x) +
                                         Mathf.Abs(block.position.z - player.position.z);

                if (distanceToPlayer > maxDistanceToPlayer)
                {
                    maxDistanceToPlayer = distanceToPlayer;
                    farthestBlock = block;
                }
            }
        }

        if (farthestBlock != null)
        {
            if (recentBlocks.Count == 2)
            {
                recentBlocks.Dequeue(); // Remove the oldest block
            }
            recentBlocks.Enqueue(farthestBlock); // Add the new block to the queue
            targetBlock = farthestBlock;
            Debug.Log("Moving away from the player via block: " + targetBlock.name);
        }
    }

    // Method to move the AI towards a guided spot
    private void MoveToGuidedSpot()
    {
        if (guidedSpots.Length > 0)
        {
            Transform chosenSpot = guidedSpots[Random.Range(0, guidedSpots.Length)];
            Transform nearestBlockToSpot = FindNearestBlockToPosition(chosenSpot.position);

            if (nearestBlockToSpot != null)
            {
                if (recentBlocks.Count == 2)
                {
                    recentBlocks.Dequeue();
                }
                recentBlocks.Enqueue(nearestBlockToSpot);
                targetBlock = nearestBlockToSpot;
                Debug.Log("Moving towards guided spot via block: " + targetBlock.name);
            }
        }
    }

    // Method to charge towards the player
    private void ChargeTowardsPlayer()
    {
        Transform nearestBlockToPlayer = FindNearestBlockToPosition(player.position);

        if (nearestBlockToPlayer != null)
        {
            if (recentBlocks.Count == 2)
            {
                recentBlocks.Dequeue();
            }
            recentBlocks.Enqueue(nearestBlockToPlayer);
            targetBlock = nearestBlockToPlayer;
            Debug.Log("Charging towards player via block: " + targetBlock.name);
        }
    }

    // Method to find the next block that brings the AI closer to the player
    private void FindNextBlockTowardsPlayer()
    {
        Transform nearestBlock = null;
        float minDistanceToPlayer = float.MaxValue;

        foreach (Transform block in blocks)
        {
            // Skip blocks that are unpassable, recently visited, occupied, or occupied by the player
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block) || IsBlockOccupiedByPlayer(block))
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

    // Method to find the nearest block to a given position
    private Transform FindNearestBlockToPosition(Vector3 position)
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

            float distance = Vector3.Distance(block.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBlock = block;
            }
        }

        return nearestBlock;
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
                // Skip blocks that are unpassable, recently visited, occupied, or occupied by the player
                if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block) || IsBlockOccupiedByPlayer(block))
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

        // Use increased speed if the player is nearby
        float currentSpeed = IsPlayerNearby() ? increasedSpeed : movementSpeed;

        // Calculate the journey length and start time
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        Debug.Log("Moving towards: " + targetBlock.name);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Check if the AI is colliding with an unpassable block
            if (IsCollidingWithUnpassable())
            {
                Debug.Log("Collided with an unpassable block. Stopping movement.");
                isMoving = false;
                yield break; // Exit the coroutine
            }

            float distanceCovered = (Time.time - startTime) * currentSpeed;
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

    // Method to smoothly face the player
    private void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the angle to the player
        float angle = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        // Snap the angle to the nearest 90 degrees (0, 90, 180, 270)
        float snappedAngle = Mathf.Round(angle / 90) * 90;

        // Apply the snapped rotation
        transform.rotation = Quaternion.Euler(0, snappedAngle, 0);
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

    private bool IsBlockOccupiedByPlayer(Transform block)
    {
        Collider[] colliders = Physics.OverlapSphere(block.position, 0.1f); // Check for objects in the block's position
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player")) // If an object tagged "Player" is found, the block is occupied
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPathBlocked(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        // Perform a raycast to check for unpassable blocks
        RaycastHit hit;
        if (Physics.Raycast(startPosition, direction, out hit, distance))
        {
            if (hit.collider.CompareTag("Unpassable"))
            {
                return true; // Path is blocked by an unpassable block
            }
        }

        return false; // Path is clear
    }

    private bool IsCollidingWithUnpassable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust radius as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Unpassable"))
            {
                return true; // Colliding with an unpassable block
            }
        }
        return false;
    }

  
}