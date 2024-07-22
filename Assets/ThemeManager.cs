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
                StartCoroutine(FadeInTheme(theme, fadeDuration));
            } else {
                audioManager.PlaySound(theme);
            }
        }
    }

    public void StopTheme(SoundType theme, bool fadeOut = false, float fadeDuration = 1.0f) {
        if (activeThemes.Contains(theme)) {
            if (fadeOut) {
                StartCoroutine(FadeOutTheme(theme, fadeDuration));
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

    private IEnumerator FadeInTheme(SoundType theme, float duration) {
        audioManager.PlaySound(theme);
        AudioSource source = audioManager.GetAudioSource(theme);
        source.volume = 0;
        float targetVolume = soundBoard.GetSound(theme).volume;

        while (source.volume < targetVolume) {
            source.volume += targetVolume * Time.deltaTime / duration;
            yield return null;
        }
    }

    private IEnumerator FadeOutTheme(SoundType theme, float duration) {
        AudioSource source = audioManager.GetAudioSource(theme);
        float startVolume = source.volume;

        while (source.volume > 0) {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioManager.StopSound(theme);
        activeThemes.Remove(theme);
    }
}
