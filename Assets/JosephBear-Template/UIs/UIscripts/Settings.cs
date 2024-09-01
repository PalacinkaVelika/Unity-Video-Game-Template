using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : UIBehaviour {
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;


    public void UpdateUISettingsElements() {
        UpdateResolutionOnStart();
        UpdateQualityOnStart();
        UpdateSliderValuesOnStart();
    }

    void CreateResolutionOptions() {
        Resolution[] resolutions = SettingsManager.Instance.GetResolutions();
        resolutionDropdown.ClearOptions();
        List<string> resolutionList = new List<string>();
        foreach (Resolution resolution in resolutions) {
            string option = resolution.width + " x " + resolution.height + ": " + resolution.refreshRate + "Hz";
            resolutionList.Add(option);
        }
        resolutionDropdown.AddOptions(resolutionList);
    }

    void UpdateResolutionOnStart() {
        CreateResolutionOptions();
        resolutionDropdown.value = SettingsManager.Instance.GetCurrentResolutionIndex();
    }

    void UpdateQualityOnStart() {
        qualityDropdown.value = SettingsManager.Instance.GetQuality();
    }

    void UpdateSliderValuesOnStart() {
        MasterSlider.value = SettingsManager.Instance.GetVolumeMaster();
        MusicSlider.value = SettingsManager.Instance.GetVolumeMusic();
        EffectsSlider.value = SettingsManager.Instance.GetVolumeEffects();
    }

    public void OnResolutionChange(int level) {
        SettingsManager.Instance.SetResolution(level);
        SettingsManager.Instance.SaveSettings();
    }

    public void OnQualityChange(int level) {
        SettingsManager.Instance.SetQuality(level);
        SettingsManager.Instance.SaveSettings();
    }

    public void OnFullscreenToggle(bool fullscreen) {
        SettingsManager.Instance.SetFullscreen(fullscreen);
        SettingsManager.Instance.SaveSettings();
    }

    public void OnVolumeMasterChange(float value) {
        SettingsManager.Instance.SetVolumeMaster(value);
        SettingsManager.Instance.SaveSettings();
    }

    public void OnVolumeEffectsChange(float value) {
        SettingsManager.Instance.SetVolumeEffects(value);
        SettingsManager.Instance.SaveSettings();
    }

    public void OnVolumeMusicChange(float value) {
        SettingsManager.Instance.SetVolumeMusic(value);
        SettingsManager.Instance.SaveSettings();
    }

    public void GoBack() {
        UImanager.Instance.ShowSavedUI();
        UImanager.Instance.HideUI(UIType.Settings);
    }

    public override void Hide() {
        UtilityUI.Fade(canvas, false, 0f);
    }

    public override void Show() {
        UtilityUI.Fade(canvas, true, 0f);
        UpdateUISettingsElements();
    }
}



[System.Serializable]
public class SettingsData {
    public int resolutionIndex;
    public int qualityLevel;
    public bool isFullscreen;
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;
}
