using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyManager : MonoBehaviour
{
    public GameObject[] dummies; // Assign your dummies in the Inspector
    public float activeDuration = 10f; // Time each dummy stays active
    public float totalDuration = 60f; // Total time for the whole process
    public int points = 0;
    public TextMeshProUGUI pointsText; // Points display
    public TextMeshProUGUI timerText;  // Timer display
    public GameObject timer;
    public GameObject GameObject;
    public GameObject commonchest;
    public GameObject rarechest;
    public GameObject epicchest;

    private float remainingTime; // Tracks the remaining time
    private bool isChallengeActive = false; // Tracks if the challenge is ongoing

    private void Start()
    {
        if (dummies.Length > 0)
        {
            remainingTime = totalDuration;
        }
        else
        {
            Debug.LogWarning("No dummies assigned to DummyManager.");
        }
    }

    private IEnumerator ActivateRandomDummy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration && isChallengeActive)
        {
            // Deactivate all dummies
            foreach (GameObject dummy in dummies)
            {
                dummy.SetActive(false);
            }

            // Pick a random dummy
            int randomIndex = Random.Range(0, dummies.Length);
            dummies[randomIndex].SetActive(true);

            // Wait for the duration
            yield return new WaitForSeconds(activeDuration);

            // Update elapsed time
            elapsedTime += activeDuration;
        }

        // Deactivate all dummies after the total duration
        foreach (GameObject dummy in dummies)
        {
            dummy.SetActive(false);
        }

        Rewards();
        timer.SetActive(false);
        GameObject.SetActive(false);

        Debug.Log("Process completed. All dummies are now inactive.");
        isChallengeActive = false;
    }

    private IEnumerator TimerTick()
    {
        while (remainingTime > 0 && isChallengeActive)
        {
            UpdateTimerText();
            yield return new WaitForSeconds(1f); // Wait for 1 second
            remainingTime -= 1f; // Reduce remaining time by 1 second
        }

        if (remainingTime <= 0)
        {
            Debug.Log("Time is up!");
            isChallengeActive = false;
            Rewards();
            timer.SetActive(false);
            GameObject.SetActive(false);
        }
    }

    public void StartChallenge()
    {
        remainingTime = totalDuration; // Reset the timer
        UpdateTimerText(); // Ensure the timer starts fresh
        isChallengeActive = true;
        StartCoroutine(ActivateRandomDummy());
        StartCoroutine(TimerTick());
        GameObject.SetActive(true);
        timer.SetActive(true);
    }

    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        Debug.Log("Points: " + points);
        pointsText.text = "Points: " + points;
    }

    public void ResetPoints()
    {
        points = 0;
    }

    public void Rewards()
    {
        if (points >= 6000) // Check for epic chest first
        {
            epicchest.SetActive(true);
            commonchest.SetActive(false);
            rarechest.SetActive(false);
        }
        else if (points >= 1500) // Check for rare chest second
        {
            rarechest.SetActive(true);
            commonchest.SetActive(false);
            epicchest.SetActive(false);
        }
        else if (points >= 500) // Check for common chest last
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

    private void UpdateTimerText()
    {
        // Format the remaining time as minutes and seconds
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }
}

