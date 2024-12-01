using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itembobbing : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second.

    [Header("Floating Settings")]
    public float floatAmplitude = 0.5f; // Maximum height difference for floating.
    public float floatFrequency = 1f;  // Speed of the up-and-down motion.

    private Vector3 startPosition;

    private void Start()
    {
        // Save the initial position of the object.
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);

        // Calculate the floating motion.
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Apply the floating motion.
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
