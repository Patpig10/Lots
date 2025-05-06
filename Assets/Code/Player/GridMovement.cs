using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float movementSpeed = 5f;      // Movement speed
    public float rotationSpeed = 180f;    // Speed of the rotation in degrees per second
    public List<Transform> blocks;        // List of block transforms representing grid cells
    public Animator animator;              // Animator reference
    public AudioSource GrassS;
    public AudioSource IceS;
    public AudioSource StoneS;
    public AudioSource WoodS;

    private Transform targetBlock;         // The target block to move towards
    public bool isMoving = false;         // Check if movement is ongoing
    private Quaternion targetRotation;     // The target rotation to smoothly rotate towards
    public float searchRadius = 1.5f;      // Define the radius for searching blocks (adjacent blocks only)
    private bool isKnockbackActive = false; // Separate knockback state
    public bool Grass;
    public bool Ice;
    public bool Stone;
    public bool Wood;

    void Start()
    {
        // Initialize the blocks list
        blocks = new List<Transform>();

        // Populate the blocks list with all GrassBlock objects in the scene
        UpdateBlocksList();
    }

    void Update()
    {
        UpdateBlocksList();

        // Prevent movement if knockback is active
        if (isKnockbackActive)
            return;

        // Get left trigger value (returns 0-1)
        float leftTriggerValue = Input.GetAxis("Control1");

        // Check if either Left Shift is pressed OR left trigger is pressed past threshold
        bool isRotatingOnly = Input.GetButton("Control") || leftTriggerValue > 0.5f;

        if (isRotatingOnly)
        {
            Debug.Log("Rotating only - Control: " + Input.GetButton("Control") +
                    ", Trigger: " + leftTriggerValue);
            HandleShiftRotation();
        }
        else if (!isMoving)
        {
            HandleNormalMovement();
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleShiftRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Only process input if there's significant input to avoid accidental rotation
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            Vector3 input = Vector3.zero;

            // Prioritize the stronger input axis
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                input = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            }
            else
            {
                input = new Vector3(0, 0, Mathf.Sign(verticalInput));
            }

            RotatePlayerSmooth(input);
        }
    }

    private void HandleNormalMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float deadzone = 0.5f; // Adjust this threshold as needed
        Vector3 input = Vector3.zero;

        // Only process input if above deadzone to avoid accidental joystick drift
        if (Mathf.Abs(horizontalInput) > deadzone && Mathf.Abs(verticalInput) < deadzone)
        {
            input = new Vector3(Mathf.Sign(horizontalInput), 0, 0);  // Horizontal only
        }
        else if (Mathf.Abs(verticalInput) > deadzone && Mathf.Abs(horizontalInput) < deadzone)
        {
            input = new Vector3(0, 0, Mathf.Sign(verticalInput));  // Vertical only
        }

        // Enforce single-axis movement (no diagonal movement)
        if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) == 0)
        {
            input = new Vector3(horizontalInput, 0, 0);  // Only horizontal movement
        }
        else if (Mathf.Abs(verticalInput) > 0 && Mathf.Abs(horizontalInput) == 0)
        {
            input = new Vector3(0, 0, verticalInput);  // Only vertical movement
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

                Debug.Log("Moving to block: " + targetBlock.name);
                StartCoroutine(MoveToBlock());
            }
            else
            {
                Debug.Log("No valid block found in direction: " + input);
            }
        }
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
            if (block == null || !block.gameObject.activeSelf)
            {
                continue; // Skip destroyed or inactive blocks
            }

            // Calculate the distance from the player to the block
            float distanceToBlock = Vector3.Distance(transform.position, block.position);

            // Only consider blocks within the search radius (adjacent blocks)
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
                        nearestBlock = block;
                    }
                }
            }
        }

        if (nearestBlock != null && !nearestBlock.CompareTag("Unpassable")) // **Check here**
        {
            targetBlock = nearestBlock;
            return true; // Found a valid target block to move towards
        }

        targetBlock = null; // Reset if no valid block is found
        return false; // No valid block found
    }

    public IEnumerator MoveToBlock()
    {
        isMoving = true;
        if (Grass == true)
        {
            GrassS.Play();
        }
        if (Ice == true)
        {
            IceS.Play();
        }
        if (Stone == true)
        {
            StoneS.Play();
        }
        if (Wood == true)
        {
            WoodS.Play();
        }

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

        transform.position = targetPosition; // Snap to the final position

        // Check if the target block has a portal
        Portal portal = targetBlock.GetComponent<Portal>();
        if (portal != null)
        {
            portal.TeleportPlayer();  // Teleport the player using the portal
        }

        isMoving = false;
        Debug.Log("Movement completed to block: " + targetBlock.name);
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

    // Method to update the blocks list dynamically
    public void UpdateBlocksList()
    {
        // Clear the current list
        blocks.Clear();

        // Find all active GrassBlock objects in the scene and add them to the list
        GrassBlock[] grassBlocks = FindObjectsOfType<GrassBlock>();
        foreach (GrassBlock block in grassBlocks)
        {
            if (block.gameObject.activeSelf)
            {
                blocks.Add(block.transform);
            }
        }

        Debug.Log("Blocks list updated. Total blocks: " + blocks.Count);
    }

    // Helper method to detect gamepad connection
    private bool IsGamepadConnected()
    {
        string[] joysticks = Input.GetJoystickNames();
        return joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]);
    }
}