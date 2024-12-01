using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class GridSystem : MonoBehaviour
{
    public float gridSize = 1f;       // Size of each grid square
    public float movementSpeed = 5f;  // Movement speed
    public int visibleGridRange = 5;  // Number of grids visible around the player

    private Vector3 targetPosition;   // Target position on the grid
    private bool isMoving = false;    // Check if movement is ongoing

    void Start()
    {
        // Initialize the target position to the player's current position, snapped to the grid
        targetPosition = SnapToGrid(transform.position);
    }

    void Update()
    {
        // Only allow input if the player isn't currently moving
        if (!isMoving)
        {
            // Get player input for horizontal (X axis) and vertical (Z axis) movement
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // Ensure input is non-zero and calculate the new target position
            if (input != Vector3.zero)
            {
                // Calculate the new target position based on input
                Vector3 newPosition = targetPosition + input * gridSize;

                // Snap to the nearest grid position
                targetPosition = SnapToGrid(newPosition);
                StartCoroutine(MovePlayer());  // Start smooth movement to the new position
            }
        }

        // Draw the debug grid lines
        DrawDebugGrid();
    }

    // Coroutine for smooth movement to the new target position
    private IEnumerator MovePlayer()
    {
        isMoving = true;  // Set the flag to prevent movement while transitioning
        Vector3 startPosition = transform.position;
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

    // Method to snap a position to the nearest grid point
    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = position.y; // Keep the Y position unchanged
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }

    // Method to draw a limited grid around the player
    void DrawDebugGrid()
    {
        // Get the player's current grid position
        Vector3 playerPosition = transform.position;

        // Calculate the starting and ending grid positions to draw the grid within the visible range
        int minX = Mathf.FloorToInt(playerPosition.x / gridSize) - visibleGridRange;
        int maxX = Mathf.FloorToInt(playerPosition.x / gridSize) + visibleGridRange;
        int minZ = Mathf.FloorToInt(playerPosition.z / gridSize) - visibleGridRange;
        int maxZ = Mathf.FloorToInt(playerPosition.z / gridSize) + visibleGridRange;

        // Draw the grid lines only within the player's visible range
        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                // XZ plane (horizontal grid lines)
                Debug.DrawLine(new Vector3(x * gridSize, 0, minZ * gridSize), new Vector3(x * gridSize, 0, maxZ * gridSize), Color.green);
                Debug.DrawLine(new Vector3(minX * gridSize, 0, z * gridSize), new Vector3(maxX * gridSize, 0, z * gridSize), Color.green);
            }
        }
    }

    // Draw gizmos in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        DrawGridGizmos();
    }

    // Method to draw gizmos for the grid
    private void DrawGridGizmos()
    {
        // Get the player's current grid position
        Vector3 playerPosition = transform.position;

        // Calculate the starting and ending grid positions to draw the grid within the visible range
        int minX = Mathf.FloorToInt(playerPosition.x / gridSize) - visibleGridRange;
        int maxX = Mathf.FloorToInt(playerPosition.x / gridSize) + visibleGridRange;
        int minZ = Mathf.FloorToInt(playerPosition.z / gridSize) - visibleGridRange;
        int maxZ = Mathf.FloorToInt(playerPosition.z / gridSize) + visibleGridRange;

        // Draw the grid lines only within the player's visible range
        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                // XZ plane (horizontal grid lines)
                Gizmos.DrawLine(new Vector3(x * gridSize, 0, minZ * gridSize), new Vector3(x * gridSize, 0, maxZ * gridSize));
                Gizmos.DrawLine(new Vector3(minX * gridSize, 0, z * gridSize), new Vector3(maxX * gridSize, 0, z * gridSize));
            }
        }
    }
}
