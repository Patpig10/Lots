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
    public float dashSpeed = 10f;         // Speed during dash
    public float dashDuration = 0.5f;     // Duration of the dash

    private Transform targetBlock;        // The next block to move towards
    public bool isMoving = false;        // Check if movement is ongoing
    private Queue<Transform> recentBlocks = new Queue<Transform>(); // Queue to track the last two visited blocks
    private bool isDashing = false;      // Check if the Lich is currently dashing

    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float stuckThreshold = 3f; // 3 seconds threshold
    public Transform spawnPoint; // Reference to the spawn point
    private Rigidbody rb;

    void Start()
    {
        lastPosition = transform.position; // Initialize the last position
        StartCoroutine(ChangeTargetRoutine());
            rb = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        isMoving = false;
        targetBlock = null;
        lastPosition = transform.position; // Reset the last position
        StartCoroutine(ChangeTargetRoutine());
    }

    void Update()
    {
        // Check if the Lich is stuck
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime; // Increment the stuck timer
        }
        else
        {
            stuckTimer = 0f; // Reset the stuck timer if the Lich is moving
        }

        // If stuck for 3 seconds, try to unstick
        if (stuckTimer >= stuckThreshold)
        {
            Debug.Log("Lich is stuck. Attempting to unstick.");
            MoveToUnstick();
            stuckTimer = 0f; // Reset the stuck timer
        }

        // If stuck for an extended period (e.g., 10 seconds), teleport to spawn
        if (stuckTimer >= 10f) // Adjust the duration as needed
        {
            Debug.Log("Lich has been stuck for too long. Teleporting to spawn.");
            TeleportToSpawn();
            stuckTimer = 0f; // Reset the stuck timer
        }

        lastPosition = transform.position; // Update the last position

        if (!isMoving && !isDashing)
        {
            if (IsPlayerNearby()) // If the player is nearby, calculate the path to move away, to a guided spot, or charge
            {
                if (ShouldRunAway() && Random.value < runAwayChance)
                {
                    FindNextBlockAwayFromPlayer();
                }
                else if (Random.value < chargeChance)
                {
                   // StartCoroutine(DashTowardsPlayer());
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
            else // If the player is not nearby, roam randomly
            {
                FindRandomBlock();
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

    private void TeleportToSpawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            Debug.Log("Lich was stuck for too long. Teleported to spawn point.");
        }
        else
        {
            Debug.LogWarning("Spawn point not assigned. Cannot teleport.");
        }
    }
    // Method to move the Lich 1 unit to the left
    private void MoveToUnstick()
    {
        // Try moving left, right, forward, and backward in sequence
        Vector3[] directions = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        foreach (Vector3 direction in directions)
        {
            Vector3 newPosition = transform.position + direction;
            if (IsPositionValid(newPosition))
            {
                transform.position = newPosition;
                Debug.Log("Unstuck by moving in direction: " + direction);
                return; // Exit after the first valid move
            }
        }

        Debug.Log("Failed to unstick. All directions blocked.");
    }


    // Method to check if a position is valid (not blocked by unpassable blocks)
    private bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Unpassable"))
            {
                Debug.Log("Position is blocked by an unpassable block: " + collider.name);
                return false; // Position is blocked by an unpassable block
            }
        }
        return true; // Position is valid
    }

    // Coroutine to choose a new block at regular intervals when the player is not nearby
    private IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            if (!IsPlayerNearby()) // Only find a new block if the player isn't nearby
            {
                FindRandomBlock();
            }
            yield return new WaitForSeconds(1f); // Shorter interval for smoother behavior
        }
    }

    // Method to detect if the player is within the detection radius
    private bool IsPlayerNearby()
    {
        if (Vector3.Distance(transform.position, player.position) > detectionRadius)
            return false;

        // Perform a raycast from Lich to Player
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.collider.CompareTag("Unpassable"))
            {
                return false; // Player is behind an unpassable block
            }
        }

        return true; // Player is visible
    }


    // Method to check if the player is within the run-away radius
    private bool ShouldRunAway()
    {
        return Vector3.Distance(transform.position, player.position) <= runAwayRadius;
    }

    // Method to find a random block to move to (for roaming)
    private void FindRandomBlock()
    {
        List<Transform> validBlocks = new List<Transform>();

        foreach (Transform block in blocks)
        {
            // Skip unpassable blocks and recently visited blocks
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block))
            {
                continue;
            }

            // Check if the path to the block is clear
            if (IsPathClear(transform.position, block.position))
            {
                validBlocks.Add(block);
            }
        }

        if (validBlocks.Count > 0)
        {
            Transform randomBlock = validBlocks[Random.Range(0, validBlocks.Count)];
            UpdateRecentBlocks(randomBlock);
            targetBlock = randomBlock;
            Debug.Log("Roaming to random block: " + targetBlock.name);
        }
    }

    // Method to find the next block that moves the AI away from the player
    private void FindNextBlockAwayFromPlayer()
    {
        Transform farthestBlock = null;
        float maxDistanceToPlayer = float.MinValue;

        foreach (Transform block in blocks)
        {
            // Skip unpassable blocks and recently visited blocks
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block) || IsBlockOccupiedByPlayer(block))
            {
                continue;
            }

            // Check if the block is within the maxRunAwayDistance
            float distanceFromBoss = Vector3.Distance(block.position, transform.position);
            if (distanceFromBoss > maxRunAwayDistance)
            {
                continue;
            }

            // Check alignment and distance to player
            if (Mathf.Abs(block.position.x - transform.position.x) < 0.1f ||
                Mathf.Abs(block.position.z - transform.position.z) < 0.1f)
            {
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
            UpdateRecentBlocks(farthestBlock);
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
                UpdateRecentBlocks(nearestBlockToSpot);
                targetBlock = nearestBlockToSpot;
                Debug.Log("Moving towards guided spot via block: " + targetBlock.name);
            }
        }
    }

    // Coroutine to dash towards the player
    private IEnumerator DashTowardsPlayer()
    {
        // Avoid dash if the Lich is already too close to the player
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            yield break;
        }

        // Check if the path to the player is clear
        if (!IsPathClear(transform.position, player.position))
        {
            Debug.Log("Path to player is blocked. Cancelling dash.");
            yield break; // Exit the coroutine if the path is blocked
        }

        isDashing = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GetValidPosition(player.position); // Ensure the target position is valid
        targetPosition.y = startPosition.y;

        float startTime = Time.time;

        while (Time.time - startTime < dashDuration)
        {
            // Check if the path ahead is blocked
            if (!IsPathClear(transform.position, targetPosition))
            {
                Debug.Log("Path blocked during dash. Stopping.");
                break;
            }

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the Lich reaches the exact target position
        transform.position = targetPosition;
        isDashing = false;
        Debug.Log("Dash completed.");
    }

    // Method to find the next block that brings the AI closer to the player
    private void FindNextBlockTowardsPlayer()
    {
        Transform nearestBlock = null;
        float minDistanceToPlayer = float.MaxValue;

        foreach (Transform block in blocks)
        {
            // Skip unpassable blocks and recently visited blocks
            if (block.CompareTag("Unpassable") || recentBlocks.Contains(block) || IsBlockOccupied(block) || IsBlockOccupiedByPlayer(block))
            {
                continue;
            }

            // Check alignment and distance to player
            if (Mathf.Abs(block.position.x - transform.position.x) < 0.1f ||
                Mathf.Abs(block.position.z - transform.position.z) < 0.1f)
            {
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
            UpdateRecentBlocks(nearestBlock);
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
            // Skip unpassable blocks and recently visited blocks
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
                // Skip unpassable blocks and recently visited blocks
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
                UpdateRecentBlocks(nearestBlock);
                targetBlock = nearestBlock;
                Debug.Log("New target block set: " + targetBlock.name);
            }
        }
    }

    // Coroutine for smooth movement to the target block
    private IEnumerator MoveToBlock()
    {
        if (targetBlock == null || IsBlockOccupied(targetBlock)) // Check if the target block is valid
        {
            Debug.Log("Target block is invalid or occupied. Stopping movement.");
            isMoving = false;
            yield break;
        }

        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GetValidPosition(targetBlock.position); // Ensure the target position is valid
        targetPosition.y = startPosition.y; // Keep Y axis fixed

        float currentSpeed = IsPlayerNearby() ? increasedSpeed : movementSpeed;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float journeyTime = journeyLength / currentSpeed;
        float elapsedTime = 0f;

        Debug.Log("Moving towards block: " + targetBlock.name);

        while (elapsedTime < journeyTime)
        {
            // Check for collisions with unpassable blocks
            if (IsCollidingWithUnpassable())
            {
                Debug.Log("Collided with an unpassable block. Stopping movement.");
                isMoving = false;
                yield break; // Exit the coroutine
            }

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / journeyTime;
            t = Mathf.SmoothStep(0f, 1f, t); // Smooth interpolation

            // Move using Rigidbody for physics-based movement
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            GetComponent<Rigidbody>().MovePosition(newPosition);

            yield return null;
        }

        // Ensure it reaches the exact position
        GetComponent<Rigidbody>().MovePosition(targetPosition);
        Debug.Log("Reached block: " + targetBlock.name);

        yield return new WaitForSeconds(pauseDuration);
        isMoving = false;
    }
    private Vector3 GetValidPosition(Vector3 targetPosition)
    {
        // Check if the target position is valid
        if (IsPositionValid(targetPosition))
        {
            return targetPosition;
        }

        // If the target position is invalid, find the nearest valid position
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Step back until a valid position is found
        for (float step = 0.1f; step < distance; step += 0.1f)
        {
            Vector3 newPosition = transform.position + direction * step;
            if (IsPositionValid(newPosition))
            {
                return newPosition;
            }
        }

        // If no valid position is found, return the current position
        return transform.position;
    }
    // Method to smoothly face the player with blocky rotation
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
        Collider[] colliders = Physics.OverlapSphere(block.position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Unpassable"))
            {
                return true;
            }
        }
        return false;
    }

    // Method to check if a block is occupied by the player
    private bool IsBlockOccupiedByPlayer(Transform block)
    {
        Collider[] colliders = Physics.OverlapSphere(block.position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    // Method to check if the AI is colliding with an unpassable block
    private bool IsCollidingWithUnpassable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Unpassable"))
            {
                Debug.Log("Colliding with unpassable block: " + collider.name);
                return true;
            }
        }
        return false;
    }

    // Method to check if the path between two positions is clear
    private bool IsPathClear(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        // Use a sphere cast to check for obstacles along the path
        float radius = 0.5f; // Adjust based on the size of the Lich
        RaycastHit hit;
        if (Physics.SphereCast(start, radius, direction, out hit, distance))
        {
            if (hit.collider.CompareTag("Unpassable"))
            {
                Debug.Log("Path is blocked by an unpassable block: " + hit.collider.name);
                return false; // Path is blocked by an unpassable block
            }
        }

        return true; // Path is clear
    }

    // Helper method to update the recent blocks queue
    private void UpdateRecentBlocks(Transform newBlock)
    {
        if (recentBlocks.Count == 2)
        {
            recentBlocks.Dequeue();
        }
        recentBlocks.Enqueue(newBlock);
    }

    public void StopMovement()
    {
        isMoving = true;
    }

    public void ResumeMovement()
    {
        isMoving = false;
    }
}