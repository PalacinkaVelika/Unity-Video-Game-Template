using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : UIBehaviour {
    
    public GameObject canvas;
    public AudioMixer mixer;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;

    Resolution[] resolutions;

    private void Start() {
        CreateResolutions();
        UpdateQualityOnStart();
        UpdateSliderValuesOnStart();
        // i want it to defaulty not suck :)
        SetFullscreen(true);
        SetResolution(resolutions.Length-1);
    }
    // Sets up UI resolution dropdown
    void CreateResolutions() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionList = new List<string>();
        int index = 0;
        int currentResIdx = 0;
        foreach (Resolution resolution in resolutions) {
            string opt = resolution.width + " x " + resolution.height;
            resolutionList.Add(opt);
            if(Screen.currentResolution.width == resolution.width && 
                Screen.currentResolution.height == resolution.height) {
                currentResIdx = index;
            }
            index++;
        }
        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.value = currentResIdx;
        resolutionDropdown.RefreshShownValue();
    }

    void UpdateQualityOnStart() {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }
    void UpdateSliderValuesOnStart() {
        float currentVolume;
        mixer.GetFloat("Master", out currentVolume);
        MasterSlider.value = UnMixerizeAudioValue(currentVolume);
        mixer.GetFloat("Music", out currentVolume);
        MusicSlider.value = UnMixerizeAudioValue(currentVolume);
        mixer.GetFloat("Effects", out currentVolume);
        EffectsSlider.value = UnMixerizeAudioValue(currentVolume);
    }

    public void SetResolution(int level) {
        Screen.SetResolution(resolutions[level].width, resolutions[level].height, Screen.fullScreen);
        print("setting res: " + Screen.currentResolution);
    }

    public void SetQuality(int level) {
        QualitySettings.SetQualityLevel(level);
        print("setting qual: " + QualitySettings.GetQualityLevel());
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
        print("setting fs: " + Screen.fullScreen);
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

    float MixerizeAudioValue(float value) {
        float minDecibels = -80f;
        float maxDecibels = 5f;
        return minDecibels + (maxDecibels - minDecibels) * value;
    }

    public float UnMixerizeAudioValue(float decibels) {
        float minDecibels = -80f;
        float maxDecibels = 5f;
        decibels = Mathf.Clamp(decibels, minDecibels, maxDecibels);
        float normalizedValue = (decibels - minDecibels) / (maxDecibels - minDecibels);
        return normalizedValue;
    }

    // Otevøe naposledy otevøené UI (to které basically zavolalo tohle okno)
    public void GoBack() {
        UImanager.Instance.ShowSavedUI();
        UImanager.Instance.HideUI(UIType.Settings);
    }

    public override void Hide() {
        UtilityUI.Fade(canvas, false, 0f);
    }

    public override void Show() {
        UtilityUI.Fade(canvas, true, 0f);
    }
}