using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : MonoBehaviour
{
    public GameObject[] dummies; // Assign your dummies in the Inspector
    public float activeDuration = 10f; // Time each dummy stays active
    public float totalDuration = 60f; // Total time for the whole process

    private void Start()
    {
        if (dummies.Length > 0)
        {
           // StartCoroutine(ActivateRandomDummy());
        }
        else
        {
            Debug.LogWarning("No dummies assigned to DummyManager.");
        }
    }

    private IEnumerator ActivateRandomDummy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
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

        Debug.Log("Process completed. All dummies are now inactive.");
    }

    public void StartChallege()
    {
        StartCoroutine(ActivateRandomDummy());
    }
}
