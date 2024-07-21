using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInitializer : MonoBehaviour, Iinitializer {

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
        GameManager.Instance.ChangeGameState(GameState.Running);
    }

}
