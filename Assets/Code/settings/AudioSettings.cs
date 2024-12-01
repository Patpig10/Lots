using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_Text masterVolumeText;
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;

    [Header("Audio Mixer")]
    public UnityEngine.Audio.AudioMixer audioMixer;

    void Start()
    {
        // Load saved settings or use default values
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // Apply initial settings
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);

        // Add listeners for real-time updates
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMasterVolume(float value)
    {
        ApplyVolume("MasterVolume", value);
        masterVolumeText.text = $"Master: {(int)(value * 100)}%";
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void SetMusicVolume(float value)
    {
        ApplyVolume("MusicVolume", value);
        musicVolumeText.text = $"Music: {(int)(value * 100)}%";
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void SetSFXVolume(float value)
    {
        ApplyVolume("SFXVolume", value);
        sfxVolumeText.text = $"SFX: {(int)(value * 100)}%";
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void ApplyVolume(string parameter, float value)
    {
        if (value <= 0.01f)
        {
            // If the slider is near zero, set volume to -80 dB (mute)
            audioMixer.SetFloat(parameter, -80f);
        }
        else
        {
            // Convert slider value (0 to 1) to logarithmic scale (-80 dB to 0 dB)
            float dB = Mathf.Log10(value) * 20;
            audioMixer.SetFloat(parameter, dB);
        }
    }
}
