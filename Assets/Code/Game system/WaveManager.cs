using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this namespace for scene management

public class WaveManager : MonoBehaviour
{
    // Each wave is a public array of enemies
    public GameObject[] wave1Enemies;
    public GameObject[] wave2Enemies;
    public GameObject[] wave3Enemies;
    public GameObject[] wave4Enemies;

    public int currentWave = 0;
    public float intermissionTime = 60f; // 1-minute intermission
    public GameObject shopUI;
    public Announcer announcer;
    public string sceneToLoadAfterWave4; // Name of the scene to load after Wave 4

    private bool isIntermission = false;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (currentWave < 4) // 4 waves total
        {
            // Announce the wave
            AnnounceWave();

            // Spawn enemies for the current wave
            yield return StartCoroutine(SpawnEnemies(GetCurrentWaveEnemies()));

            // End of wave
            Debug.Log("Wave " + (currentWave + 1) + " completed!");
            currentWave++;

            // Intermission
            if (currentWave < 4)
            {
                yield return StartCoroutine(Intermission());
            }
        }

        Debug.Log("All waves completed!");

        // Teleport to the specified scene after Wave 4
        if (!string.IsNullOrEmpty(sceneToLoadAfterWave4))
        {
            SceneManager.LoadScene(sceneToLoadAfterWave4);
        }
        else
        {
            Debug.LogWarning("No scene specified to load after Wave 4.");
        }
    }

    void AnnounceWave()
    {
        switch (currentWave)
        {
            case 0:
                announcer.ShowMessage("Are you ready to see our bravest warriors to battle?");
                break;
            case 1:
                announcer.ShowMessage("Troops of the Ice Kingdom wish to join in the fun!");
                break;
            case 2:
                announcer.ShowMessage("Let the speedy brothers show what's up!");
                break;
            case 3:
                announcer.ShowMessage("The Giants with a big heart fighting the newcomer!");
                break;
        }
    }

    IEnumerator SpawnEnemies(GameObject[] waveEnemies)
    {
        foreach (var enemy in waveEnemies)
        {
            if (enemy != null) // Ensure the enemy is not destroyed
            {
                enemy.SetActive(true);
                yield return new WaitForSeconds(1f); // Delay between enemy spawns
            }
        }

        // Wait until all enemies are defeated or destroyed
        while (AreEnemiesRemaining(waveEnemies))
        {
            yield return null;
        }
    }

    IEnumerator Intermission()
    {
        Debug.Log("Intermission started! You have 1 minute to get ready!");
        isIntermission = true;
        shopUI.SetActive(true); // Activate shop UI
        yield return new WaitForSeconds(intermissionTime); // Wait for intermission duration
        shopUI.SetActive(false); // Deactivate shop UI
        isIntermission = false;
        Debug.Log("Intermission ended!");
    }

    bool AreEnemiesRemaining(GameObject[] waveEnemies)
    {
        foreach (var enemy in waveEnemies)
        {
            // If the enemy is not null and is active in the hierarchy, it's still remaining
            if (enemy != null && enemy.activeInHierarchy)
            {
                return true; // At least one enemy is still active
            }
        }
        // All enemies are either destroyed or inactive
        return false;
    }

    // Function to start the next wave manually
    public void StartNextWave()
    {
        if (isIntermission)
        {
            StopAllCoroutines();
            shopUI.SetActive(false);
            isIntermission = false;
            StartCoroutine(WaveLoop());
        }
    }

    // Helper method to get the current wave's enemies
    GameObject[] GetCurrentWaveEnemies()
    {
        switch (currentWave)
        {
            case 0: return wave1Enemies;
            case 1: return wave2Enemies;
            case 2: return wave3Enemies;
            case 3: return wave4Enemies;
            default: return null;
        }
    }
}