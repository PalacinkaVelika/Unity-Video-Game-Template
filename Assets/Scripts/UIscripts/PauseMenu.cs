using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PauseMenu : UIBehaviour {
    public static PauseMenu Instance { get; private set; }

    public GameObject canvas;
    bool canPause = false;
    bool isPaused = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        InputManager.Instance?.SubscribeToAction("Pause", OnPause);
    }

    public override void Hide() {
        UImanager.Instance.ToggleAllButtonsInUI(UIType.PauseMenu, true);
        canvas.SetActive(false);
    }

    public override void Show() {
        canvas.SetActive(true);
    }

    void OnPause(InputAction.CallbackContext context) {
        if (canPause) {
            Pause();
        }
    }

    public void Pause() {
        if (!isPaused) {
            UImanager.Instance.ShowUI(UIType.PauseMenu);
            GameManager.Instance.ChangeGameState(GameState.Paused);
            InputManager.Instance?.SubscribeToAction("Pause", OnPause);
            AudioManager.Instance.PauseAllSounds();
            ThemeManager.Instance.PauseAllThemes();
            Time.timeScale = 0f;
            isPaused = true;
        } else {
            UnPause();
        }
    }
    void UnPause() {
        UImanager.Instance.HideUI(UIType.PauseMenu);
        UImanager.Instance.HideUI(UIType.Settings);
        Time.timeScale = 1.0f;
        GameManager.Instance.ChangeToPreviousGameState();
        AudioManager.Instance.ResumeAllSounds();
        ThemeManager.Instance.ResumeAllThemes();
        isPaused = false;
    }
    public void GoToSettings() {
        UImanager.Instance.SaveOpenedUI();
        UImanager.Instance.ShowUI(UIType.Settings);
        UImanager.Instance.HideUI(UIType.PauseMenu);
    }

    public void GoToMenu() {
        UImanager.Instance.ToggleAllButtonsInUI(UIType.PauseMenu, false);
        StartCoroutine(GoMenu());
    }

    IEnumerator GoMenu() {
        Time.timeScale = 1.0f;
        UImanager.Instance.ShowUI(UIType.LoadingScreen);
        yield return new WaitForSeconds(.5f);
        // unload gameplay scenes
        var unloadTask = SceneLoadingManager.Instance.UnLoadAllGameplayScenes();
        yield return new WaitUntil(() => unloadTask.IsCompleted);
        if (!unloadTask.IsCompletedSuccessfully) {
            print("unloading all gameplay scenes fucked up");
            yield break;
        }
        // load main menu scene
        float loadingScreenLength = 1f; // Default loading screen length 
        print("Trying to load the scene");
        var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(SceneType.MainMenuScene, loadingScreenLength);
        yield return new WaitUntil(() => loadTask.IsCompleted);
        if (loadTask.Result) {
            UnPause();
            UImanager.Instance.HideUI(UIType.LoadingScreen);
        } else {
            Debug.LogError("Failed to load main menu scene.");
        }
    }


    public void ExitGame() {
        Application.Quit();
    }


    public void SetCanPause(bool value) {
        canPause = value;
    }

}
