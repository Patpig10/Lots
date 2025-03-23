using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPush : MonoBehaviour
{
    public Transform[] gridBlocks;  // Array of grid block transforms
    public float movementSpeed = 5f; // Speed at which the block moves
    private Vector3 targetPosition; // Target position for the block
    public bool isMoving = false; // Check if the block is currently moving
    private float stuckTimer = 0f;    // Timer to detect if the block is stuck
    public float stuckThreshold = 1f; // How long the block can be stuck before taking action
    public GameObject Player;
   public Animator PlayerAnimator;
    void Start()
    {
        //Look for gameobject with the name Slime Knight
       Player = GameObject.Find("Slime Knight");
        //Get Slime Knight Animator
        PlayerAnimator = Player.GetComponent<Animator>();
        //Populate the blocks array with all GrassBlock objects in the scene
        GrassBlock[] grassBlocks = FindObjectsOfType<GrassBlock>();
        gridBlocks = new Transform[grassBlocks.Length];

        for (int i = 0; i < grassBlocks.Length; i++)
        {
            gridBlocks[i] = grassBlocks[i].transform; // Add the transform of each GrassBlock to the blocks array
        }
    }

    void Update()
    {
        // If the block is not moving, increment the stuck timer
        if (!isMoving)
        {
            stuckTimer += Time.deltaTime;

            // If stuck too long, reset or try another action
            if (stuckTimer > stuckThreshold)
            {
                // Disable or remove the HandleStuckBlock call to prevent automatic movement
                // HandleStuckBlock();
                stuckTimer = 0f; // Reset stuck timer
            }
        }
    }

    public IEnumerator MoveBlock(Vector3 direction)
    {

        isMoving = true;  // Block is moving

        // Calculate the target grid position by adding move direction
        targetPosition = transform.position + direction;

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

    // New functions for moving in specific directions
    public void MoveForward()
    {
        PlayerAnimator.SetTrigger("Push");
        if (!isMoving)
        {
            StartCoroutine(MoveBlock(Vector3.forward));
            Debug.Log("Moving forward");
        }
    }

    public void MoveBackward()
    {
        PlayerAnimator.SetTrigger("Push");
        if (!isMoving)
        {

            StartCoroutine(MoveBlock(Vector3.back));
            Debug.Log("Moving backward");
        }
    }

    public void MoveLeft()
    {
        PlayerAnimator.SetTrigger("Push");

        if (!isMoving)
        {

            StartCoroutine(MoveBlock(Vector3.left));
            Debug.Log("Moving left");
        }
    }

    public void MoveRight()
    {
       PlayerAnimator.SetTrigger("Push");
        if (!isMoving)
        {
            

            StartCoroutine(MoveBlock(Vector3.right));
            Debug.Log("Moving right");
        }
    }
}