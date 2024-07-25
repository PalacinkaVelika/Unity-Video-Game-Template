using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : IObservable {
    public static GameManager Instance { get; private set; }
    GameState previousState;
    GameState currentState;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    public void ChangeGameState(GameState state) {
        previousState = currentState;
        currentState = state;
        NotifyObservers(currentState);
    }

    public void ChangeToPreviousGameState() {
        if (previousState != null) ChangeGameState(previousState);
    }
}

public enum GameState {
    Running,
    Paused, // pause menu, alert box(tutorial)
    MainMenu,
    Cutscene,
    Dialogue,
}