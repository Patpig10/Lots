using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this namespace for UI components

public class WaveManager : MonoBehaviour
{
    public GameObject[] wave1Enemies;
    public GameObject[] wave2Enemies;
    public GameObject[] wave3Enemies;
    public GameObject[] wave4Enemies;

    public int currentWave = 0;
    public float intermissionTime = 60f;
    public GameObject shopUI;
    public Announcer announcer;
    public string sceneToLoadAfterWave4;
    public Saving Saving;

    private bool isIntermission = false;

    void Start()
    {
        StartCoroutine(StartTournament());
        Saving = GameObject.Find("Saving").GetComponent<Saving>();
    }

    IEnumerator StartTournament()
    {
        // Initial dialogue
        announcer.ShowMessage("Ladies and gentlemen, welcome to the 500th Flamechaser Tournament!");
        yield return new WaitForSeconds(3f); // Wait for the message to display

        // Conditional dialogue based on IceEmblem
        if (Saving.Iceemblem)
        {
            announcer.ShowMessage("We have an exciting new challenger—one who has already bested the Icy Queen and the Forest Guardian! This will be a battle to remember!");
        }
        else
        {
            announcer.ShowMessage("A new challenger has entered the arena, chosen by the Forest Queen herself. I can't wait to see what this little slime is capable of!");
        }
        yield return new WaitForSeconds(4f); // Wait for the message to display

        // Final dialogue
        announcer.ShowMessage("To claim victory, you must survive all four waves of fierce combat and then face our grand champion—who remains a mystery for now.");
        yield return new WaitForSeconds(4f); // Wait for the message to display

        announcer.ShowMessage("Let the tournament begin!");
        yield return new WaitForSeconds(2f); // Wait for the message to display

        // Start the wave loop after the dialogue finishes
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (currentWave < 4)
        {
            AnnounceWave();
            yield return StartCoroutine(SpawnEnemies(GetCurrentWaveEnemies()));

            Debug.Log("Wave " + (currentWave + 1) + " completed!");
            currentWave++;

            if (currentWave < 4)
            {
                yield return StartCoroutine(Intermission());
            }
        }

        Debug.Log("All waves completed!");

        if (!string.IsNullOrEmpty(sceneToLoadAfterWave4))
        {
            Saving.Fire();
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
                Debug.Log("Wave 1: Bravest Warriors");
                break;
            case 1:
                announcer.ShowMessage("Troops of the Ice Kingdom wish to join in the fun!");
                Debug.Log("Wave 2: Ice Kingdom Troops");
                break;
            case 2:
                announcer.ShowMessage("Let the speedy brothers show what's up!");
                Debug.Log("Wave 3: Speedy Brothers");
                break;
            case 3:
                announcer.ShowMessage("The Giants with a big heart fighting the newcomer!");
                Debug.Log("Wave 4: Giants");
                break;
        }
    }

    IEnumerator SpawnEnemies(GameObject[] waveEnemies)
    {
        foreach (var enemy in waveEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Debug.LogWarning("An enemy in the wave is null!");
            }
        }

        while (AreEnemiesRemaining(waveEnemies))
        {
            yield return null;
        }
    }

    IEnumerator Intermission()
    {
        Debug.Log("Intermission started! You have 1 minute to get ready!");
        isIntermission = true;
        shopUI.SetActive(true);
        yield return new WaitForSeconds(intermissionTime);
        shopUI.SetActive(false);
        isIntermission = false;
        Debug.Log("Intermission ended!");
    }

    bool AreEnemiesRemaining(GameObject[] waveEnemies)
    {
        foreach (var enemy in waveEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    public void StartNextWave()
    {
        if (isIntermission)
        {
            StopCoroutine(Intermission());
            shopUI.SetActive(false);
            isIntermission = false;
            StartCoroutine(WaveLoop());
        }
    }

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