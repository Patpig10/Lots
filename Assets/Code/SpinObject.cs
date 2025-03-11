using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Speed of rotation in degrees per second
    public float spinSpeed = 100f;

    void Update()
    {
        // Rotate the object around the Z-axis
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}