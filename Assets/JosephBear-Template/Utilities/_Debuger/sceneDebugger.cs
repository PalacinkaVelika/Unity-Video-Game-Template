using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneDebugger : MonoBehaviour {

    public SceneField Utility;

    void Awake() {
        // load in utility cuz thats needed
        // call the Initializator to start up the scene
        StartCoroutine(LoadUtilitiesAndInitializeScene());
    }

    IEnumerator LoadUtilitiesAndInitializeScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Utilities", LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }
        Iinitializer initializer = FindInitializerInScene(SceneManager.GetActiveScene().name);
        initializer?.Initialize();
        initializer?.StartRunning();
    }

    Iinitializer FindInitializerInScene(string scene) {
        GameObject[] rootObjects = SceneManager.GetSceneByName(scene).GetRootGameObjects();
        foreach (GameObject obj in rootObjects) {
            Iinitializer initializer = obj.GetComponentInChildren<Iinitializer>();
            if (initializer != null) {
                return initializer;
            }
        }
        return null;
    }

}
