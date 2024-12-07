using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Setting : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public GameObject settingsPanel;  // The panel holding the settings UI

    private Resolution[] resolutions;
    private bool isPaused = false;
    public GameObject Heart;
    public GameObject box;

    void Start()
    {
        // Hide the settings panel at the start
        settingsPanel.SetActive(false);
        Heart.SetActive(true);
        box.SetActive(true);

        // Initialize and populate resolutions (unique by width/height)
        resolutions = Screen.resolutions
            .Select(res => new Resolution { width = res.width, height = res.height })
            .Distinct()
            .ToArray();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions.Select(r => $"{r.width}x{r.height}").ToList());

        // Check and populate quality levels
        string[] qualityNames = QualitySettings.names;
        if (qualityNames.Length > 1)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(qualityNames.ToList());
        }
        else
        {
            Debug.LogWarning("Only one quality level found. Add more levels in Project Settings > Quality.");
        }

        // Set UI elements to match current settings
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        fullscreenToggle.isOn = Screen.fullScreen;

        // Add listeners to apply and save changes immediately
        resolutionDropdown.onValueChanged.AddListener(index => SetResolution(index));
        qualityDropdown.onValueChanged.AddListener(index => SetQuality(index));
        fullscreenToggle.onValueChanged.AddListener(isFullscreen => SetFullscreen(isFullscreen));

        // Load saved settings on startup
        LoadSettings();
    }

    void Update()
    {
        // Toggle settings menu when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index); // Save instantly
    }

    void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index); // Save instantly
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0); // Save instantly
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                return i;
        }
        return 0; // Default to the first option if no match is found
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            qualityDropdown.value = PlayerPrefs.GetInt("Quality");
            QualitySettings.SetQualityLevel(qualityDropdown.value);
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
    }

    void PauseGame()
    {
        // Show settings panel and pause the game
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;  // Freeze game time
        isPaused = true;
        Heart.SetActive(false);
        box.SetActive(false);
    }

    public void ResumeGame()
    {
        // Hide settings panel and resume the game
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
        isPaused = false;
        Heart.SetActive(true);
        box.SetActive(true);
    }
    public void Quit()
    {
        // If we are running in a standalone build of the game

        Application.Quit();



        //UnityEditor.EditorApplication.isPlaying = false;
        // UnityEditor.EditorApplication.isPlaying = false;

    }
}