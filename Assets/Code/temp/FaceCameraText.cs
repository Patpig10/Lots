using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;

        // If no main camera is found, log an error
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    void Update()
    {
        // Ensure the text always faces the camera
        if (mainCamera != null)
        {
            // Set the text's rotation to match the camera's fixed angle
            transform.rotation = Quaternion.Euler(14.7199955f, 0, 0);

            // Make the text face the camera by rotating around the Y-axis
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Keep the text upright

            if (directionToCamera != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-directionToCamera) * Quaternion.Euler(14.7199955f, 0, 0);
            }
        }
    }
}

