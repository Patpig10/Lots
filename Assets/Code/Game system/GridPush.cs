using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPush : MonoBehaviour
{
    public Transform[] gridBlocks;  // Array of grid block transforms
    public float movementSpeed = 5f; // Speed at which the block moves
    public float pushDistance = 1f;  // Minimum distance for the block to start moving when pushed
    private bool isBeingPushed = false; // Flag to check if the block is being pushed
    private Vector3 targetPosition; // Target position for the block
    private bool isMoving = false; // Check if the block is currently moving
    private float stuckTimer = 0f;    // Timer to detect if the block is stuck
    public float stuckThreshold = 1f; // How long the block can be stuck before taking action

    void Start()
    {
        // Populate the blocks array with all GrassBlock objects in the scene
        GrassBlock[] grassBlocks = FindObjectsOfType<GrassBlock>();
        gridBlocks = new Transform[grassBlocks.Length];

        for (int i = 0; i < grassBlocks.Length; i++)
        {
            gridBlocks[i] = grassBlocks[i].transform; // Add the transform of each GrassBlock to the blocks array
        }
    }

    void Update()
    {
        // If the block is being pushed, detect player input
        if (isBeingPushed && !isMoving)
        {
            Vector3 playerDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // Only push if player is not too close to the block
            if (playerDirection != Vector3.zero && Vector3.Distance(transform.position, playerDirection) > pushDistance)
            {
                StartCoroutine(MoveBlock(playerDirection));
            }
            else
            {
                // If the block is not moving, increment the stuck timer
                stuckTimer += Time.deltaTime;

                // If stuck too long, reset or try another action
                if (stuckTimer > stuckThreshold)
                {
                    HandleStuckBlock();
                    stuckTimer = 0f; // Reset stuck timer
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when the player is near the block (the player needs to push it)
        if (other.CompareTag("Player"))
        {
            isBeingPushed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop pushing when the player leaves the block's proximity
        if (other.CompareTag("Player"))
        {
            isBeingPushed = false;
        }
    }

    public IEnumerator MoveBlock(Vector3 direction)
    {
        isMoving = true;  // Block is moving
        Vector3 moveDirection = Vector3.zero;

        // Only move in cardinal directions
        if (direction.x > 0)        // Moving right
            moveDirection = Vector3.right;
        else if (direction.x < 0)   // Moving left
            moveDirection = Vector3.left;
        else if (direction.z > 0)   // Moving forward
            moveDirection = Vector3.forward;
        else if (direction.z < 0)   // Moving backward
            moveDirection = Vector3.back;

        // Calculate the target grid position by adding move direction
        if (moveDirection != Vector3.zero)
        {
            targetPosition = transform.position + moveDirection;

            // Snap the target position to the nearest grid block
            Transform targetBlock = FindNearestGridBlock(targetPosition);
            if (targetBlock != null && !targetBlock.CompareTag("Unpassable"))
            {
                // Lock the Y-axis to avoid sinking
                targetPosition = new Vector3(targetBlock.position.x, transform.position.y, targetBlock.position.z);

                // Move the block to the new grid position smoothly
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                    yield return null;
                }

                // Snap to the target grid position with locked Y
                transform.position = targetPosition;

                // Reset the stuck timer because it successfully moved
                stuckTimer = 0f;
            }
        }

        isMoving = false;  // Movement completed
    }

    private Transform FindNearestGridBlock(Vector3 targetPosition)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestGridBlock = null;

        // Check for the nearest grid block
        foreach (Transform gridBlock in gridBlocks)
        {
            // Skip destroyed blocks (null references)
            if (gridBlock == null)
                continue;

            float distance = Vector3.Distance(targetPosition, gridBlock.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGridBlock = gridBlock;
            }
        }

        return closestGridBlock;
    }

    private void HandleStuckBlock()
    {
        // Perform some action if the block has been stuck for too long
        Debug.Log("Block is stuck. Taking action...");

        // Try to move the block in a random direction
        Vector3 randomDirection = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)).normalized;
        StartCoroutine(MoveBlock(randomDirection));
    }
}