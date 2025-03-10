using UnityEngine;
using UnityEngine.UI;

public class RotateUIImage : MonoBehaviour
{
    // Reference to the UI Image component
    public Image uiImage;

    // Speed of rotation (degrees per second)
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the UI Image around the Z-axis
        uiImage.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}