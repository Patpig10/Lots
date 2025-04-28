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
    public Toggle vsyncToggle; // <-- New VSync toggle
    public GameObject settingsPanel;

    private Resolution[] resolutions;
    private bool isPaused = false;
    public GameObject Heart;
    public GameObject box;

    void Start()
    {
        settingsPanel.SetActive(false);
        Heart.SetActive(true);
        box.SetActive(true);

        resolutions = Screen.resolutions
            .Select(res => new Resolution { width = res.width, height = res.height })
            .Distinct()
            .ToArray();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions.Select(r => $"{r.width}x{r.height}").ToList());

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

        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        fullscreenToggle.isOn = Screen.fullScreen;

        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;

        resolutionDropdown.onValueChanged.AddListener(index => SetResolution(index));
        qualityDropdown.onValueChanged.AddListener(index => SetQuality(index));
        fullscreenToggle.onValueChanged.AddListener(isFullscreen => SetFullscreen(isFullscreen));
        vsyncToggle.onValueChanged.AddListener(isVSyncOn => SetVSync(isVSyncOn)); // <-- Listen to VSync toggle

        // Load saved settings
        LoadSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
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
        PlayerPrefs.SetInt("Resolution", index);
    }

    void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    void SetVSync(bool isVSyncOn)
    {
        QualitySettings.vSyncCount = isVSyncOn ? 1 : 0; // 1 = VSync ON, 0 = OFF
        PlayerPrefs.SetInt("VSync", isVSyncOn ? 1 : 0);
    }

    int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                return i;
        }
        return 0;
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

        if (PlayerPrefs.HasKey("VSync"))
        {
            bool isVSyncOn = PlayerPrefs.GetInt("VSync") == 1;
            vsyncToggle.isOn = isVSyncOn;
            QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;
        }
    }

    void PauseGame()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Heart.SetActive(false);
        box.SetActive(false);
        CursorManager.Instance.ShowCursor();
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Heart.SetActive(true);
        box.SetActive(true);
        CursorManager.Instance.HideCursor();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
