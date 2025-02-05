using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
  //  public static LevelManager Instance; // Singleton instance for easy access
    public Saving Saving;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject buttonboss;
    private void Awake()
    {


      
        
    }

    public void Update()
    {

        if (Saving.levelUnlocked == 1)
        {
            button1.SetActive(true);
            button2.SetActive(false);
            button3.SetActive(false);
            button4.SetActive(false);
            buttonboss.SetActive(false);
        }
        if (Saving.levelUnlocked >= 2)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(false);
            button4.SetActive(false);
            buttonboss.SetActive(false);
        }

        if (Saving.levelUnlocked == 3)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(false);
            buttonboss.SetActive(false);
        }

        if (Saving.levelUnlocked == 4)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            buttonboss.SetActive(false);
        }

        if (Saving.levelUnlocked == 5)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
            button4.SetActive(true);
            buttonboss.SetActive(true);
        }
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
        LoadLevel(2);
    }
    public void Level2()
    {
        LoadLevel(6);
    }
    public void Level3()
    {
        LoadLevel(8);
    }
    public void Level4()
    {
        LoadLevel(4);
    }
    public void Level5()
    {
        LoadLevel(5);
    }

   
}
