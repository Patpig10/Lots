using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dodgemanager : MonoBehaviour
{
    [Header("Orb Settings")]
    public GameObject orbPrefab; // Prefab for the orbs to spawn
    public List<Transform> spawnPoints; // List of points where orbs spawn
    public float spawnInterval = 8f; // Time interval between spawns

    [Header("Shooter Settings")]
    public List<GameObject> shooters; // List of shooters to activate/deactivate
    public float shooterSwitchInterval = 5f; // Interval to switch active shooters

    [Header("Game Settings")]
    public float gameDuration = 60f; // Duration of the mini-game
    public int rewardPoints = 100; // Points awarded for completing the mini-game

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText; // UI element to display the score
    public TextMeshProUGUI timerText; // UI element to display the timer
    public GameObject Timer;
    public GameObject points;

    private bool isGameActive = false; // Flag to check if the game is active
    private Coroutine spawnCoroutine;
    private Coroutine shooterCoroutine;
    private float gameTimer;
    public int currentScore = 0; // Tracks the player's score
    public GameObject commonchest;
    public GameObject rarechest;
    public GameObject epicchest;

    private void Start()
    {
        // Ensure all shooters are initially inactive
        foreach (var shooter in shooters)
        {
            shooter.SetActive(false);
        }

        // Initialize UI
        UpdateScoreUI();
        UpdateTimerUI();
    }

    public void StartGame()
    {
        isGameActive = true;
        gameTimer = gameDuration;
        currentScore = 0; // Reset score at the start of the game

        shooterCoroutine = StartCoroutine(SwitchActiveShooters());
        spawnCoroutine = StartCoroutine(SpawnOrbs());
        Timer.SetActive(true);
        points.SetActive(true);
        StartCoroutine(GameTimer());
    }

    public void EndGame()
    {
        isGameActive = false;
        foreach (var shooter in shooters)
        {
            shooter.SetActive(false);
        }
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        if (shooterCoroutine != null)
        {
            StopCoroutine(shooterCoroutine);
        }
        Debug.Log("Game Over! Total Score: " + currentScore);
        Timer.SetActive(false);
        points.SetActive(false);
        Rewards();
    }

    private IEnumerator SpawnOrbs()
    {
        while (isGameActive)
        {
            SpawnOrb();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOrb()
    {
        if (orbPrefab != null && spawnPoints.Count > 0)
        {
            // Choose a random spawn point from the list
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Instantiate(orbPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Orb prefab or spawn points not set!");
        }
    }

    private IEnumerator GameTimer()
    {
        while (gameTimer > 0 && isGameActive)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        if (isGameActive)
        {
            currentScore += rewardPoints; // Add reward points
            Debug.Log("Game Complete! Reward Points: " + rewardPoints);
        }

        EndGame();
    }

    private IEnumerator SwitchActiveShooters()
    {
        while (isGameActive)
        {
            // Deactivate all shooters
            foreach (var shooter in shooters)
            {
                shooter.SetActive(false);
            }

            // Activate 5 random shooters
            List<int> chosenIndices = new List<int>();
            for (int i = 0; i < 5 && i < shooters.Count; i++)
            {
                int index;
                do
                {
                    index = Random.Range(0, shooters.Count);
                } while (chosenIndices.Contains(index));
                chosenIndices.Add(index);
                shooters[index].SetActive(true);
            }

            yield return new WaitForSeconds(shooterSwitchInterval);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();
        Debug.Log("Current Score: " + currentScore);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(gameTimer) + "s";
        }
    }

    public void Rewards()
    {
        if (currentScore >= 500) // Check for epic chest first
        {
            epicchest.SetActive(true);
            commonchest.SetActive(false);
            rarechest.SetActive(false);
        }
        else if (currentScore >= 150) // Check for rare chest second
        {
            rarechest.SetActive(true);
            commonchest.SetActive(false);
            epicchest.SetActive(false);
        }
        else if (currentScore >= 50) // Check for common chest last
        {
            commonchest.SetActive(true);
            rarechest.SetActive(false);
            epicchest.SetActive(false);
        }
        else
        {
            Debug.Log("You need more points to claim a reward.");
            commonchest.SetActive(false);
            rarechest.SetActive(false);
            epicchest.SetActive(false);
        }
    }
}
