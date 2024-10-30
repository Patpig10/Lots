using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // For changing the heart colors


public class HeartSystem : MonoBehaviour
{
    public GameObject heartPrefab;  // Prefab of the heart (UI Image or Sprite)
    public Transform heartContainer;  // Container where hearts will be spawned
    public int maxLife = 3;  // Starting number of hearts
    private int life;
    private List<GameObject> hearts = new List<GameObject>();  // List to store spawned hearts
    public float heartSpacing = 10f;
    private bool dead;
    public Saving save;
    private void Start()
    {
        save = GameObject.FindObjectOfType<Saving>();
        maxLife = save.maxSavedLife;
        life = maxLife;  // Start with full life
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

            // Set the anchored position of the new heart with the customizable spacing
            heartRectTransform.anchoredPosition = new Vector2(i * (heartRectTransform.rect.width + heartSpacing), 0);
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
        }
    }

    // Method to change the color of all hearts (blue for shield active, white for normal)
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
}
