using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PauseMenu : UIBehaviour {
    public static PauseMenu Instance { get; private set; }
    public SceneField menuScene;
    public SceneField gameplayScene;

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
        StartCoroutine(GoMenu());
    }

    IEnumerator GoMenu() {
        UnPause();
        Fader.Instance.Fade(true, .5f);
        yield return new WaitForSeconds(.7f);
        SceneLoadingManager.Instance.UnLoadScene(gameplayScene); // THIS MUST BE CHANGED FOR THE DYNAMIC STUFF TO WORK
        var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(menuScene, 1f);
        yield return new WaitForSeconds(.7f); // wait just for the lols

        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (loadTask.Result) {
            Fader.Instance.Fade(false, .5f);
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
