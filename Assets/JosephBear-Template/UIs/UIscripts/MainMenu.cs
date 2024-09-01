using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIBehaviour {

    public static MainMenu Instance { get; private set; }

    bool loading = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public override void Hide() {
        UtilityUI.ToggleAllButtonsInCanvas(UImanager.Instance.GetCanvasFromUI(UIType.MainMenu), true);
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
            UtilityUI.ToggleAllButtonsInCanvas(UImanager.Instance.GetCanvasFromUI(UIType.MainMenu), false);
            StartCoroutine(LoadNewGameplay());
        }
    }

    public void LoadGame() {
        if (!loading) {
            loading = true;
            UtilityUI.ToggleAllButtonsInCanvas(UImanager.Instance.GetCanvasFromUI(UIType.MainMenu), false);
            StartCoroutine(LoadSavedGame()); // Will load the save file and hold on to it
        }
    }

    public void GoToSettings() {
        UImanager.Instance.SaveOpenedUI(UIType.MainMenu);
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

    IEnumerator LoadNewGameplay() {
        // Clear saved data
        SaveManager.Instance.ClearSaveData();
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