using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelmenu : MonoBehaviour
{
   
    private void Awake()
    {




    }

    public void Update()
    {

    }

    // Load a new scene by name
    public void LoadLevel(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found!");
        }
    }

    // Load a scene by its build index
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

    // Reload the current scene
    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load the next level based on the build index
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! You’ve finished the game.");
        }
    }

    // Exit the game (only works in builds)
    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void Level1()
    {
        LoadLevel(3);
    }
    public void Level2()
    {
        LoadLevel(2);
    }
    public void Level3()
    {
        LoadLevel(3);
    }
    public void Level4()
    {
        LoadLevel(4);
    }
    public void Level5()
    {
        LoadLevel(5);
    }
    public void Map()
    {
        LoadLevel(1);
    }
    public void MainMenu()
    {
        LoadLevel(0);
    }
    public void hat()
    {
        LoadLevel(4);
    }

    public void Startscence()
    {
        // Load the first level when the game starts
        LoadLevel(22);
    }
}

