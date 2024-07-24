using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {
    public static GameInitializer Instance { get; private set; }
    public Animator UIanim;
    bool mainMenuLoaded = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            InitializeGame();
        } else {
            Destroy(gameObject);
        }
    }

    // What happens after the game is turned on
    void InitializeGame() {
        StartCoroutine(LoadUtilitiesAndInitializeGame());
    }

    IEnumerator LoadUtilitiesAndInitializeGame() {
        // Tady musím použít unity scene manager abych naèetl mùj og scene manager
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Utilities", LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        StartCoroutine(InitializeGameC());
    }

    IEnumerator InitializeGameC() {
        PlayLogoAnimation();
        yield return new WaitForSeconds(0.2f);
        var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(SceneType.MainMenuScene, UIanim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitUntil(() => loadTask.IsCompleted);
        if (loadTask.Result) {
            UtilityUI.Fade(UIanim.gameObject, false);
        } else {
            Debug.LogError("Failed to load main menu scene.");
        }
        
    }

    void PlayLogoAnimation() {
        UIanim.SetTrigger("show");
    }


}
