using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The player's transform to follow
    public Vector3 offset;          // The offset distance between the player and the camera
    public float smoothSpeed = 0.125f;  // Smoothing factor for the camera movement

    private void LateUpdate()
    {
        // Desired position based on target position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position but keep its rotation unchanged
        transform.position = smoothedPosition;
    }
}
