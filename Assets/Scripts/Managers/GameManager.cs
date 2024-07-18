using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IObservable {
    public static GameManager Instance { get; private set; }
    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    GameState currentState;

    public void ChangeGameState(GameState state) {
        NotifyObservers(currentState);
    }

}

public enum GameState {
    Running,
    Paused, // pause menu, alert box(tutorial)
    MainMenu,
    Cutscene,
    Dialogue,
}