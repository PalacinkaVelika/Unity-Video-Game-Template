using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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


    public async Task<bool> LoadSceneAsync(SceneField scene, float waitAfterInitialization = 1f) {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(LoadSceneAsyncC(scene, tcs, waitAfterInitialization));
        return await tcs.Task;
    }

    public void LoadScene(SceneField scene) {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        loadedScenes.Add(scene);
    }

    public void UnLoadScene(SceneField scene) {
        SceneManager.UnloadSceneAsync(scene);
        if (loadedScenes.Contains(scene)) loadedScenes.Remove(scene);
    }

    IEnumerator LoadSceneAsyncC(SceneField scene, TaskCompletionSource<bool> tcs, float waitAfterInitialization) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            yield return null;
        }
        loadedScenes.Add(scene);
        tcs.SetResult(true);
        // Call the scenes Initializator
        Iinitializer initializer = FindInitializerInScene(scene);
        initializer?.Initialize();
        yield return new WaitForSeconds(waitAfterInitialization);
        initializer?.StartRunning();
    }

    Iinitializer FindInitializerInScene(SceneField scene) {
        GameObject[] rootObjects = SceneManager.GetSceneByName(scene.SceneName).GetRootGameObjects();
        foreach (GameObject obj in rootObjects) {
            Iinitializer initializer = obj.GetComponentInChildren<Iinitializer>();
            if (initializer != null) {
                return initializer;
            }
        }
        return null;
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
