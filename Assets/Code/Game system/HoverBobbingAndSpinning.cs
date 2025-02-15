using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBobbingAndSpinning : MonoBehaviour
{
    public float hoverHeight = 0.5f;  // Height of the bobbing
    public float hoverSpeed = 2f;     // Speed of bobbing
    public float rotateSpeed = 50f;   // Speed of rotation
    public float floorHeight = 0f;    // Height of the floor (adjust as needed)

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Hovering and bobbing effect using sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Ensure the object doesn't go below the floor
        if (newY < floorHeight)
        {
            newY = floorHeight;
        }

        // Apply the new position to the object (keep X and Z constant, change Y)
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotating effect (rotation around the Y-axis)
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
