using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    public GameObject heartPrefab;  // Prefab of the heart (UI Image or Sprite)
    public Transform heartContainer;  // Container where hearts will be spawned
    public int maxLife = 7;  // Starting number of hearts
    private int life;
    private List<GameObject> hearts = new List<GameObject>();  // List to store spawned hearts
    public float heartSpacing = 10f;
    private bool dead;
    public Saving save;
    private const int maxHeartsInRow = 8;  // Maximum number of hearts in a row

    private void Start()
    {
        save = GameObject.FindObjectOfType<Saving>();
        if (save == null)
        {
            Debug.LogError("Saving component not found!");
            return;
        }

        maxLife = save.maxSavedLife;
        life = maxLife;  // Start with full life
        Debug.Log($"HeartSystem Start: maxLife = {maxLife}, life = {life}");
        SpawnHearts();  // Dynamically spawn hearts
    }

    void Update()
    {
        if (dead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload level or show game over
        }
    }

    private void SpawnHearts()
    {
        // Clear old hearts
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();  // Clear the list of old hearts

        // Loop through and spawn hearts based on maxLife
        for (int i = 0; i < maxLife; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(newHeart);

            RectTransform heartRectTransform = newHeart.GetComponent<RectTransform>();

            // Calculate the position of the heart
            if (i < maxHeartsInRow)
            {
                // Position the first 8 hearts normally (first row)
                heartRectTransform.anchoredPosition = new Vector2(i * (heartRectTransform.rect.width + heartSpacing), 0);
            }
            else if (i < 2 * maxHeartsInRow)
            {
                // Position the next 8 hearts (9th to 16th) on top of the first row (second row)
                int overlapIndex = (i - maxHeartsInRow) % maxHeartsInRow;  // Calculate which heart to overlap
                heartRectTransform.anchoredPosition = hearts[overlapIndex].GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                // Position additional hearts (17th and beyond) on top of the second row (third row)
                int overlapIndex = (i - 2 * maxHeartsInRow) % maxHeartsInRow;  // Calculate which heart to overlap
                heartRectTransform.anchoredPosition = hearts[overlapIndex + maxHeartsInRow].GetComponent<RectTransform>().anchoredPosition;
            }
        }

        UpdateHeartVisuals();  // Refresh heart visuals after spawning
    }

    public void TakeDamage(int damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);  // Decrease life but keep it within valid bounds

        // Update heart visuals after taking damage
        UpdateHeartVisuals();

        if (life < 1)
        {
            dead = true;  // Trigger death if all hearts are gone
        }
    }

    public void AddLife(int amount)
    {
        life = Mathf.Clamp(life + amount, 0, maxLife);  // Adjust life but don't exceed maxLife
        UpdateHeartVisuals();  // Refresh heart visuals after healing
    }

    public void UpgradeMaxLife(int extraHearts)
    {
        maxLife += extraHearts;  // Increase max life
        life = Mathf.Clamp(life, 0, maxLife);  // Ensure current life doesn't exceed maxLife
        SpawnHearts();  // Respawn hearts with the new max life
        save.maxSavedLife++;
    }

    private void UpdateHeartVisuals()
    {
        // Update heart visuals based on the current life value
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < life);  // Show hearts that represent the current life

            // Change color based on the row
            if (i < maxHeartsInRow)
            {
                // First row: Red
                Image heartImage = hearts[i].GetComponent<Image>();
                if (heartImage != null)
                {
                    heartImage.color = Color.red;  // First row hearts are red
                }
            }
            else if (i < 2 * maxHeartsInRow)
            {
                // Second row: Green
                Image heartImage = hearts[i].GetComponent<Image>();
                if (heartImage != null)
                {
                    heartImage.color = Color.green;
                }
            }
            else
            {
                // Third row: Purple
                Image heartImage = hearts[i].GetComponent<Image>();
                if (heartImage != null)
                {
                    heartImage.color = new Color(0.5f, 0f, 0.5f);  // Purple color
                }
            }
        }
    }

    public void SetHeartColor(Color color)
    {
        foreach (GameObject heart in hearts)
        {
            Image heartImage = heart.GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.color = color;
            }
        }
    }

    public int GetCurrentLife()
    {
        return life;
    }

    public void FullHeal()
    {
        life = save.maxSavedLife;
        UpdateHeartVisuals();
    }
}