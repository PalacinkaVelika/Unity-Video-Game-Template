using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager: MonoBehaviour {
    public static SettingsManager Instance {  get; private set; }
    public  AudioMixer mixer;
    Resolution[] resolutions;
    int currentResolutionIndex = 0;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            resolutions = Screen.resolutions;
        }
    }


    public void SetResolution(int index) {
        if (index >= 0 && index < resolutions.Length) {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
            currentResolutionIndex = index;
        }
    }

    public int GetCurrentResolutionIndex() {
        return currentResolutionIndex;
    }

    public void SetQuality(int level) {
        QualitySettings.SetQualityLevel(level);
    }

    public int GetQuality() {
        return QualitySettings.GetQualityLevel();
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }

    public bool GetFullscreen() {
        return Screen.fullScreen;
    }

    public void SetVolumeMaster(float value) {
        mixer.SetFloat("Master", MixerizeAudioValue(value));
    }

    public void SetVolumeEffects(float value) {
        mixer.SetFloat("Effects", MixerizeAudioValue(value));
    }

    public void SetVolumeMusic(float value) {
        mixer.SetFloat("Music", MixerizeAudioValue(value));
    }

    public float GetVolumeMaster() {
        float currentVolume;
        mixer.GetFloat("Master", out currentVolume);
       return UnMixerizeAudioValue(currentVolume);
    }

    public float GetVolumeMusic() {
        float currentVolume;
        mixer.GetFloat("Music", out currentVolume);
        return UnMixerizeAudioValue(currentVolume);
    }

    public float GetVolumeEffects() {
        float currentVolume;
        mixer.GetFloat("Effects", out currentVolume);
        return UnMixerizeAudioValue(currentVolume);
    }

    public float MixerizeAudioValue(float value) {
        float minDecibels = -80f;
        float maxDecibels = 5f;
        return minDecibels + (maxDecibels - minDecibels) * value;
    }

    public float UnMixerizeAudioValue(float decibels) {
        float minDecibels = -80f;
        float maxDecibels = 5f;
        decibels = Mathf.Clamp(decibels, minDecibels, maxDecibels);
        return (decibels - minDecibels) / (maxDecibels - minDecibels);
    }

    public Resolution[] GetResolutions() {
        return resolutions;
    }

    public void ApplySettings(SettingsData settingsData) {
        print("settings applied (loaded)");
        SetFullscreen(settingsData.isFullscreen);
        SetResolution(settingsData.resolutionIndex);
        SetVolumeMaster(settingsData.masterVolume);
        SetVolumeMusic(settingsData.musicVolume);
        SetVolumeEffects(settingsData.effectsVolume);
    }

    public void SaveSettings() {
        SettingsData settingsData = new SettingsData {
            resolutionIndex = GetCurrentResolutionIndex(),
            qualityLevel = GetQuality(),
            isFullscreen = GetFullscreen(),
            masterVolume = GetVolumeMaster(),
            musicVolume = GetVolumeMusic(),
            effectsVolume = GetVolumeEffects()
        };
        SaveManager.Instance.SaveSettings(settingsData);
        print("settings saved");
    }
}
