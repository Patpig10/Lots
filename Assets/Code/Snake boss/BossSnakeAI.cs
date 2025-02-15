using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnakeAI : MonoBehaviour
{
    public Transform[] blocks;
    public GameObject bodyPrefab;
    public float movementSpeed = 3f;
    public float pauseDuration = 1f;
    public float changeTargetInterval = 5f;
    public float bodyGrowthInterval = 2f;
    public int initialBodySize = 3;
    public float blockCooldownDuration = 30f;
    public float speedIncreaseInterval = 20f;
    public float speedIncreaseAmount = 0.5f;
    public float bodyGrowthRateIncreaseAmount = 0.1f;

    private Transform targetBlock;
    private bool isMoving = false;
    private Dictionary<Transform, float> blockCooldowns = new Dictionary<Transform, float>();
    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> previousPositions = new List<Vector3>();
    private HashSet<Vector3> occupiedGridPositions = new HashSet<Vector3>();

    private float growthTimer = 0f;
    private float speedIncreaseTimer = 0f;
    private Transform player;
    private bool isChasingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the tag 'Player'.");
        }

        StartCoroutine(ChangeTargetRoutine());
        StartCoroutine(IncreaseSpeedOverTime());

        // Initialize the snake's body
        for (int i = 0; i < initialBodySize; i++)
        {
            AddBodySegment(true);
        }
    }

    void Update()
    {
        if (targetBlock != null && !isMoving)
        {
            StartCoroutine(MoveToBlock());
        }

        growthTimer += Time.deltaTime;
        if (growthTimer >= bodyGrowthInterval)
        {
            growthTimer = 0f;
            AddBodySegment(false);
        }
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);

            // Increase movement speed
            movementSpeed += speedIncreaseAmount;

            // Increase body growth rate
            bodyGrowthInterval -= bodyGrowthRateIncreaseAmount;
            if (bodyGrowthInterval < 0.1f) // Ensure it doesn't get too fast
            {
                bodyGrowthInterval = 0.1f;
            }
        }
    }

    private IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            FindNearestBlock();
            yield return new WaitForSeconds(changeTargetInterval);
        }
    }

    private void FindNearestBlock()
    {
        if (blocks.Length > 0)
        {
            float minDistance = float.MaxValue;
            Transform nearestBlock = null;

            foreach (Transform block in blocks)
            {
                if (block == null || IsBlockOnCooldown(block)) continue;

                float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                                   new Vector3(block.position.x, 0, block.position.z));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBlock = block;
                }
            }

            if (nearestBlock != null && nearestBlock != targetBlock)
            {
                targetBlock = nearestBlock;
                blockCooldowns[targetBlock] = Time.time + blockCooldownDuration;
            }
        }
    }

    private bool IsBlockOnCooldown(Transform block)
    {
        if (blockCooldowns.TryGetValue(block, out float cooldownEndTime))
        {
            return Time.time < cooldownEndTime;
        }
        return false;
    }

    private IEnumerator MoveToBlock()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetBlock.position;
        targetPosition.y = startPosition.y;

        Vector3 directionToTarget = (targetPosition - startPosition).normalized;
        float angle = 0f;

        if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.z))
        {
            angle = directionToTarget.x > 0 ? 90f : -90f;
        }
        else
        {
            angle = directionToTarget.z > 0 ? 0f : 180f;
        }

        transform.rotation = Quaternion.Euler(0, angle, 0);

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        transform.position = targetPosition;
        previousPositions.Insert(0, targetPosition);

        occupiedGridPositions.Add(transform.position);

        if (previousPositions.Count > bodyParts.Count + 1)
        {
            previousPositions.RemoveAt(previousPositions.Count - 1);
        }

        UpdateBodyMovement();

        yield return new WaitForSeconds(pauseDuration);

        isMoving = false;
    }

    private void AddBodySegment(bool isInitialSetup)
    {
        Vector3 newBodyPosition;

        if (bodyParts.Count == 0)
        {
            // Spawn the first body segment behind the head
            newBodyPosition = transform.position - transform.forward;
        }
        else
        {
            // Spawn the new body part behind the last segment
            newBodyPosition = bodyParts[bodyParts.Count - 1].position - bodyParts[bodyParts.Count - 1].forward;
        }

        // Instantiate the new body part
        Transform newBodyPart = Instantiate(bodyPrefab, newBodyPosition, Quaternion.Euler(-90f, 0f, 0f)).transform;
        bodyParts.Add(newBodyPart);

        // Track previous positions if it's initial setup
        if (isInitialSetup)
        {
            previousPositions.Add(newBodyPosition);
        }
    }

    private void UpdateBodyMovement()
    {
        if (previousPositions.Count < bodyParts.Count + 1) return;

        bodyParts.RemoveAll(part => part == null);

        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            Vector3 targetPosition = previousPositions[i + 1];

            occupiedGridPositions.Remove(bodyParts[i].position);

            bodyParts[i].position = targetPosition;

            occupiedGridPositions.Add(targetPosition);

            Vector3 direction = targetPosition - previousPositions[i];
            if (direction != Vector3.zero)
            {
                float angle = 0f;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    angle = direction.x > 0 ? -90f : 90f;
                }
                else
                {
                    angle = direction.y > 0 ? 0f : 180f;
                }
                bodyParts[i].rotation = Quaternion.Euler(-90f, 0f, angle);
            }
        }

        previousPositions.Insert(0, transform.position);

        if (previousPositions.Count > bodyParts.Count + 1)
        {
            previousPositions.RemoveAt(previousPositions.Count - 1);
        }
    }

    public void DestroyBodyPart(Transform bodyPart)
    {
        if (bodyParts.Contains(bodyPart))
        {
            int index = bodyParts.IndexOf(bodyPart);
            bodyParts.RemoveAt(index);

            if (index < previousPositions.Count)
            {
                previousPositions.RemoveAt(index + 1);
            }

            if (occupiedGridPositions.Contains(bodyPart.position))
            {
                occupiedGridPositions.Remove(bodyPart.position);
            }

            Destroy(bodyPart.gameObject);
        }
    }
}