using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour {
    public static SceneLoadingManager Instance { get; private set; }

    List<SceneField> loadedScenes = new List<SceneField>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    public async Task<bool> LoadSceneAsync(SceneField scene) {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(LoadSceneAsyncC(scene, tcs));
        return await tcs.Task;
    }

    public void LoadScene(SceneField scene) {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        loadedScenes.Add(scene);
    }

    public void UnLoadScene(SceneField scene) {
        if (loadedScenes.Contains(scene)) {
            SceneManager.UnloadSceneAsync(scene);
            loadedScenes.Remove(scene);
        }
    }

    private IEnumerator LoadSceneAsyncC(SceneField scene, TaskCompletionSource<bool> tcs) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            yield return null;
        }
        loadedScenes.Add(scene);
        tcs.SetResult(true);
    }

}

/**
 * how to load scene async externally
 * 
    private IEnumerator InitializeGameC() {
        // Wait until the scene loading task is complete
        var loadTask = sceneLoader.LoadMainMenuAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);

        if (loadTask.Result)
        {
            // after its loaded do something
        }
        else
        {
            Debug.LogError("Failed to load scene.");
        }
    }
*/
