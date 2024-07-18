using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {
    public static GameInitializer Instance { get; private set; }
    public Animator UIanim;
    public SceneField Utilities;
    public SceneField mainMenuScene;
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
        SceneLoadingManager.Instance.LoadScene(Utilities); 
        StartCoroutine(InitializeGameC());
    }
    
    IEnumerator InitializeGameC() {
        var loadTask = SceneLoadingManager.Instance.LoadSceneAsync(mainMenuScene);
        PlayLogoAnimation();
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(UIanim.GetCurrentAnimatorStateInfo(0).length);

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
