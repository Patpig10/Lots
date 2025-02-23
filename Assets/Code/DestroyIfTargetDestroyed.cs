using UnityEngine;

public class DestroyIfTargetDestroyed : MonoBehaviour
{
    public GameObject targetObject; // The object to check for destruction

    void Update()
    {
        // Check if the target object has been destroyed
        if (targetObject == null)
        {
            // Destroy this object if the target is destroyed
            Destroy(gameObject);
        }
    }
}