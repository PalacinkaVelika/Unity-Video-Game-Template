using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour, Iinitializer {

    // Deactivating all objects in the scene is only one of the possible approaches
    // If for example you just want to start gameplay right away simply deactivate playerController in Initialize instead
    private GameObject[] rootObjects;
    public void Initialize() {
        rootObjects = gameObject.scene.GetRootGameObjects();
        foreach (var obj in rootObjects) {
            obj.SetActive(false);
        }
    }

    public void StartRunning() {
        foreach (var obj in rootObjects) {
            obj.SetActive(true);
        }
        UImanager.Instance.ShowUI(UIType.MainMenu);
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
    }
}
