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

    private float lastSlamTime;
    private int activeArmIndex;        // Index of the currently slamming arm
    private Vector3[] armStartPositions; // Store the starting positions of the arms
    private bool isSlamming = false;   // Track if an arm is currently slamming

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
        // If the last two slams were the same arm, choose the other arm
        if (lastTwoSlams[0] == lastTwoSlams[1])
        {
            return 1 - lastTwoSlams[1]; // Choose the opposite arm
        }

        // Otherwise, choose a random arm
        return Random.Range(0, arms.Length);
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
