
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
}
