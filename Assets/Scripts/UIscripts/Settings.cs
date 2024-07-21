using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : UIBehaviour {
    
    public GameObject canvas;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start() {
        CreateResolutions();
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

    public void SetResolution(int level) {
        Screen.SetResolution(resolutions[level].width, resolutions[level].height, Screen.fullScreen);
    }

    public void SetQuality(int level) {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
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