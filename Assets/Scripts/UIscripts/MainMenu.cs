using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIBehaviour {

    public static MainMenu Instance { get; private set; }
    public GameObject canvas;

    bool loading = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public override void Hide() {
        UtilityUI.Fade(canvas, false, 0f);
    }

    public override void Show() {
        UtilityUI.Fade(canvas, true, .1f);
        InputManager.Instance.SwitchToUIControls();
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
    }
    


    public void NewGame() {
        if (!loading) {
            loading = true;
            StartCoroutine(LoadGameplay());
        }
    }

    public void LoadGame() {
        if (!loading) {
            loading = true;
            StartCoroutine(LoadSavedGame()); // Will load the save file and hold on to it
        }
    }

    public void GoToSettings() {
        UImanager.Instance.SaveOpenedUI();
        UImanager.Instance.ShowUI(UIType.Settings);
        UImanager.Instance.HideUI(UIType.MainMenu);
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator LoadSavedGame() {
        var task = SaveManager.Instance.LoadGameDataAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsCompleted) {
            FindAnyObjectByType<Fader>().Fade(true, 1f);
            yield return new WaitForSeconds(1.2f);
            // Loading logic
            UImanager.Instance.HideUI(UIType.MainMenu);
            SceneLoadingManager.Instance.UnLoadSceneAsync(SceneType.MainMenuScene);
            var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(SceneType.GameplayScene, 0f, true);
            yield return new WaitUntil(() => loadTask.IsCompleted);
            if (loadTask.Result) {
                // Fade out / stop loading screen once loaded in
                SaveManager.Instance.LoadGame(); // Have to let them know to load their shit
                FindAnyObjectByType<Fader>().Fade(false);
                loading = false;
            }
        }
    }

    IEnumerator LoadGameplay() {
        // Fade in / loading screen
        FindAnyObjectByType<Fader>().Fade(true, 1f);
        yield return new WaitForSeconds(1.2f);
        // Loading logic
        UImanager.Instance.HideUI(UIType.MainMenu);
        SceneLoadingManager.Instance.UnLoadSceneAsync(SceneType.MainMenuScene);
        var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(SceneType.GameplayScene, 0f, true);
        yield return new WaitUntil(() => loadTask.IsCompleted);
        if (loadTask.Result) {
            // Fade out / stop loading screen once loaded in
            FindAnyObjectByType<Fader>().Fade(false);
            loading = false;
        }
    }

}