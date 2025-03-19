using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithBody : MonoBehaviour
{
    public GameObject body; // Reference to the "body" GameObject

    private void Update()
    {
        // Check if the "body" GameObject is destroyed
        if (body == null)
        {
            Debug.Log("Body has been destroyed. Destroying this GameObject.");
            Destroy(gameObject); // Destroy this GameObject
        }
    }
}
