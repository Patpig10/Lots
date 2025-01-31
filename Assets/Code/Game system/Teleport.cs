
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
   // public Transform teleportTarget;
   // public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");

        //   player.transform.position = teleportTarget.transform.position;
    }

    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Scene index " + sceneIndex + " out of range!");
        }
    }
    public void LoadLeveltime(int sceneIndex)
    {
        StartCoroutine(LoadLevelWithDelay(sceneIndex, 3f)); // Start the coroutine with a 3-second delay
    }

    private IEnumerator LoadLevelWithDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay (3 seconds)

        if (sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex); // Load the scene after the delay
        }
        else
        {
            Debug.LogError("Scene index " + sceneIndex + " out of range!"); // Log an error if the scene index is invalid
        }
    }
}
