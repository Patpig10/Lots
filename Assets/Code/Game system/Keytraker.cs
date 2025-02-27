using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keytraker : MonoBehaviour
{
    public GameObject[] objects; // Array to hold the 3 objects
    public int keysLeft = 3; // Initial number of keys

    void Start()
    {
        // Ensure there are exactly 3 objects assigned
        if (objects.Length != 3)
        {
            Debug.LogError("Please assign exactly 3 objects in the Inspector.");
            return;
        }

        // Attach the OnObjectDestroyed method to each object's destruction event
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                ObjectHealth health = obj.AddComponent<ObjectHealth>();
                health.OnDestroyed += OnObjectDestroyed;
            }
        }
    }

    // Method called when an object is destroyed
    private void OnObjectDestroyed()
    {
        //keysLeft--; // Decrease the number of keys left
        Debug.Log("An object was destroyed. Keys left: " + keysLeft);

        if (keysLeft <= 0)
        {
            Debug.Log("All objects have been destroyed. No keys left!");
            // You can trigger additional logic here, like ending the game
        }
    }
}

// Helper class to handle object destruction
public class ObjectHealth : MonoBehaviour
{
    public delegate void DestroyedEvent();
    public event DestroyedEvent OnDestroyed;

    void OnDestroy()
    {
        // Trigger the event when the object is destroyed
        if (OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}