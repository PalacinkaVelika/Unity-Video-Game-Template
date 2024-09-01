using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayInitializer : MonoBehaviour, Iinitializer {

    private GameObject[] rootObjects;
    public void Initialize() {
        rootObjects = gameObject.scene.GetRootGameObjects();
        foreach (var obj in rootObjects) {
            obj.SetActive(false);
        }
    }

    public void StartRunning() {
        foreach (var obj in rootObjects) {
            obj.SetActive(true);
        }
        GameManager.Instance.ChangeGameState(GameState.Running);
        ThemeManager.Instance?.PlayTheme(SoundType.ThemeSongFrFr, true);
        UImanager.Instance.ShowUI(UIType.HUD);
    }

    public void Unload() {
        // Stop all sounds
        AudioManager.Instance.StopAllSounds(true);
        ThemeManager.Instance.StopAllThemes();
        UImanager.Instance.HideUI(UIType.HUD);
    }
}
