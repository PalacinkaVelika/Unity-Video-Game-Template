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
        EditManagerSettings(currentState);
    }

    public void ChangeToPreviousGameState() {
        if (previousState != null) ChangeGameState(previousState);
    }

    void EditManagerSettings(GameState state) {
        switch (state) {
            case GameState.Running:
                InputManager.Instance.SwitchToPlayerControls();
                PauseMenu.Instance.SetCanPause(true);
                break;
            case GameState.Paused:
                InputManager.Instance.SwitchToUIControls();
                PauseMenu.Instance.SetCanPause(true);
                break;
            case GameState.MainMenu:
                InputManager.Instance.SwitchToUIControls();
                PauseMenu.Instance.SetCanPause(false);
                break;
            case GameState.Cutscene:
                InputManager.Instance.SwitchToUIControls();
                PauseMenu.Instance.SetCanPause(true);
                break;
            case GameState.Dialogue:
                InputManager.Instance.SwitchToUIControls();
                PauseMenu.Instance.SetCanPause(true);
                break;

        }
    }

}

public enum GameState {
    Running,
    Paused, // pause menu, alert box(tutorial)
    MainMenu,
    Cutscene,
    Dialogue,
}