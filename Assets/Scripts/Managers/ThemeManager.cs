using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour {
    public static ThemeManager Instance { get; private set; }

    private AudioManager audioManager;
    private SoundBoard soundBoard;
    private List<SoundType> activeThemes;
    private List<SoundType> pausedThemes;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            audioManager = AudioManager.Instance;
            soundBoard = SoundBoard.Instance;
            activeThemes = new List<SoundType>();
            pausedThemes = new List<SoundType>();
        }
    }

    public void PlayTheme(SoundType theme, bool fadeIn = false, float fadeDuration = 1.0f) {
        if (!activeThemes.Contains(theme)) {
            activeThemes.Add(theme);
            if (fadeIn) {
                audioManager.PlaySound(theme, fadeDuration);
            }else {
                audioManager.PlaySound(theme);
            }
        }
    }

    public void StopTheme(SoundType theme, bool fadeOut = false, float fadeDuration = 1.0f) {
        if (activeThemes.Contains(theme)) {
            if (fadeOut) {
                audioManager.FadeOutSound(theme, fadeDuration);
            } else {
                audioManager.StopSound(theme);
                activeThemes.Remove(theme);
            }
        }
    }

    public void StopAllThemes(bool fadeOut = false, float fadeDuration = 1.0f) {
        foreach (SoundType theme in new List<SoundType>(activeThemes)) {
            StopTheme(theme, fadeOut, fadeDuration);
        }
    }

    public void PauseAllThemes() {
        pausedThemes.Clear();
        foreach (SoundType theme in activeThemes) {
            audioManager.PauseSound(theme);
            pausedThemes.Add(theme);
        }
    }

    public void ResumeAllThemes() {
        foreach (SoundType theme in pausedThemes) {
            audioManager.ResumeSound(theme);
        }
        pausedThemes.Clear();
    }
}
