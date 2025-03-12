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
    public GameObject Forest;
    public GameObject Ice;
    public GameObject fire;
    public GameObject Ice1;
    public GameObject fire1;
    public GameObject levelIce1;
    public GameObject levelFire1;
    public GameObject levelIce2;
    public GameObject levelIceBoss;
    public GameObject levelFireBoss;
    public GameObject Wall;
    public GameObject Finalcastle;
    private void Awake()
    {


      
        
    }

    void Update()
    {
        SetButtonState(button1, Saving.levelUnlocked >= 1);
        SetButtonState(button2, Saving.levelUnlocked >= 2);
        SetButtonState(button3, Saving.levelUnlocked >= 3);
        SetButtonState(button4, Saving.levelUnlocked >= 4);
        SetButtonState(buttonboss, Saving.levelUnlocked >= 5);
        SetButtonState(levelIce1, Saving.levelUnlocked >= 6);
        SetButtonState(levelFire1, Saving.levelUnlocked >= 6);
        SetButtonState(levelIce2, Saving.levelUnlocked >= 7 && Saving.ice == true);
        SetButtonState(levelIceBoss, Saving.levelUnlocked >= 8 && Saving.ice == true);
        SetButtonState(levelFireBoss, Saving.Arena == true && Saving.fire == true);



        SetButtonState(Forest, true);
        SetButtonState(Ice, Saving.levelUnlocked >= 6);
        SetButtonState(fire, Saving.levelUnlocked >= 6);
        SetButtonState(Ice1, Saving.levelUnlocked >= 6);
        SetButtonState(fire1, Saving.levelUnlocked >= 6);

        SetButtonState(Finalcastle,Saving.Final == true );

        if(Saving.Final == true)
        {
            Wall.SetActive(false);
        }


    }

    void SetButtonState(GameObject button, bool shouldBeActive)
    {
        if (button.activeSelf != shouldBeActive)
        {
            button.SetActive(shouldBeActive);
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
        LoadLevel(7);
    }
    public void Level4()
    {
        LoadLevel(8);
    }
    public void Level5()
    {
        LoadLevel(9);
    }
    public void LevelICE1()
    {
        LoadLevel(10);
    }
    public void LevelICE2()
    {
        LoadLevel(11);
    }
    public void LevelICEBOSS()
    {
        LoadLevel(12);
    }
    public void LevelFIRE1()
    {
        LoadLevel(15);
    }
    public void LevelFIREBOSS()
    {
        LoadLevel(16);
    }
    public void LevelFINAL()
    {
        LoadLevel(18);
    }

}
