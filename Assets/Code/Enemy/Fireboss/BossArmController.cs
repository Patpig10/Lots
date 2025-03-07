using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    public GameObject[] arms;          // Array of arm GameObjects (2 arms)
    public GameObject shockwavePrefab; // Prefab for the shockwave
    public float slamCooldown = 3f;    // Cooldown between slams
    public float slamDuration = 1f;    // Duration of the slam animation
    public float slamHeight = 5f;      // Height the arm moves down during the slam
    public float groundStayDuration = 7f; // Time the arm stays on the ground after slamming
    public GameObject audioSourceObject; // GameObject containing the AudioSource

    private float lastSlamTime;
    private int activeArmIndex;        // Index of the currently slamming arm
    private Vector3[] armStartPositions; // Store the starting positions of the arms
    private bool isSlamming = false;   // Track if an arm is currently slamming
    private AudioSource audioSource;   // AudioSource to play the slam sound

    private int[] lastTwoSlams = new int[2] { -1, -1 }; // Track the last two slams

    private void Start()
    {
        // Ensure there are exactly 2 arms
        if (arms.Length != 2)
        {
            Debug.LogError("There must be exactly 2 arms assigned.");
            return;
        }

        // Initialize arm starting positions
        armStartPositions = new Vector3[arms.Length];
        for (int i = 0; i < arms.Length; i++)
        {
            armStartPositions[i] = arms[i].transform.position;
        }

        // Get the AudioSource from the referenced GameObject
        if (audioSourceObject != null)
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("The referenced GameObject does not have an AudioSource component.");
            }
        }
        else
        {
            Debug.LogError("AudioSource GameObject is not assigned.");
        }

        // Activate both arms at the start
        SetActiveArms();
    }

    private void Update()
    {
        // Check if it's time to slam and no slam is currently in progress
        if (Time.time - lastSlamTime >= slamCooldown && !isSlamming)
        {
            Slam();
        }
    }

    private void SetActiveArms()
    {
        // Activate both arms
        foreach (GameObject arm in arms)
        {
            arm.SetActive(true);
        }
    }

    private void Slam()
    {
        // Choose a random arm to slam, ensuring it doesn't slam three times in a row
        activeArmIndex = ChooseArm();

        // Check if the selected arm is active
        if (!arms[activeArmIndex].activeSelf)
        {
            // If the arm is not active, skip the slam and try again after the cooldown
            lastSlamTime = Time.time;
            return;
        }

        // Update the last two slams
        lastTwoSlams[0] = lastTwoSlams[1];
        lastTwoSlams[1] = activeArmIndex;

        // Start the slam animation
        StartCoroutine(SlamAnimation(arms[activeArmIndex].transform));

        // Update the last slam time
        lastSlamTime = Time.time;
    }

    private int ChooseArm()
    {
        // Create a list of active arm indices
        List<int> activeArmIndices = new List<int>();
        for (int i = 0; i < arms.Length; i++)
        {
            if (arms[i].activeSelf)
            {
                activeArmIndices.Add(i);
            }
        }

        // If no arms are active, return -1 (no slam)
        if (activeArmIndices.Count == 0)
        {
            return -1;
        }

        // If the last two slams were the same arm, choose the other active arm
        if (lastTwoSlams[0] == lastTwoSlams[1] && activeArmIndices.Contains(1 - lastTwoSlams[1]))
        {
            return 1 - lastTwoSlams[1];
        }

        // Otherwise, choose a random active arm
        return activeArmIndices[Random.Range(0, activeArmIndices.Count)];
    }

    private System.Collections.IEnumerator SlamAnimation(Transform armTransform)
    {
        // Mark that a slam is in progress
        isSlamming = true;

        // Store the starting position of the arm
        Vector3 startPosition = armStartPositions[activeArmIndex];

        // Calculate the end position (slammed down position)
        Vector3 endPosition = startPosition + Vector3.down * slamHeight;

        float elapsedTime = 0f;

        // Move the arm down
        while (elapsedTime < slamDuration)
        {
            armTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / slamDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the arm reaches the end position
        armTransform.position = endPosition;

        // Play the slam sound using the AudioSource from the referenced GameObject
        if (audioSource != null)
        {
            Debug.Log("Playing slam sound");
            audioSource.Play(); // Play the sound
        }
        else
        {
            Debug.LogWarning("AudioSource is missing!");
        }

        // Wait for 0.03 seconds before spawning the shockwave
        yield return new WaitForSeconds(0.03f);

        // Spawn the shockwave at the slam position
        SpawnShockwave(endPosition);

        // Wait for the ground stay duration (7 seconds)
        yield return new WaitForSeconds(groundStayDuration);

        // Move the arm back up
        elapsedTime = 0f;
        while (elapsedTime < slamDuration)
        {
            armTransform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime / slamDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the arm returns to the exact start position
        armTransform.position = startPosition;

        // Mark that the slam is complete
        isSlamming = false;
    }

    private void SpawnShockwave(Vector3 position)
    {
        // Create the shockwave at the slam position
        GameObject shockwave = Instantiate(shockwavePrefab, position, Quaternion.identity);
        shockwave.transform.localScale = Vector3.one; // Reset scale if needed
    }
}