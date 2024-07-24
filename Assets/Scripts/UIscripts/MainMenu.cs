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
    void Start() {

    }

    void Update() {

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
            StartCoroutine(LoadGameplay());
            loading = true;
        }
    }

    public void LoadGame() {
        
    }

    public void GoToSettings() {
        UImanager.Instance.SaveOpenedUI();
        UImanager.Instance.ShowUI(UIType.Settings);
        UImanager.Instance.HideUI(UIType.MainMenu);
    }

    public void QuitGame() {
        Application.Quit();
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