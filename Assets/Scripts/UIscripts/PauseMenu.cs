using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PauseMenu : UIBehaviour {
    public static PauseMenu Instance { get; private set; }

    public GameObject canvas;
    bool canPause = false;
    bool isPaused = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        InputManager.Instance?.SubscribeToAction("Pause", OnPause);
    }

    public override void Hide() {
        canvas.SetActive(false);
    }

    public override void Show() {
        canvas.SetActive(true);
    }

    void OnPause(InputAction.CallbackContext context) {
        print("butt pressed");
        if (canPause) {
            Pause();
        }
    }

    public void Pause() {
        if (!isPaused) {
            Show();
            GameManager.Instance.ChangeGameState(GameState.Paused);
            InputManager.Instance?.SubscribeToAction("Pause", OnPause);
            Time.timeScale = 0f;
            isPaused = true;
        } else {
            Hide();
            Time.timeScale = 1.0f;
            GameManager.Instance.ChangeToPreviousGameState();
            isPaused = false;
        }
    }

    public void GoToSettings() {
        UImanager.Instance.SaveOpenedUI();
        UImanager.Instance.ShowUI(UIType.Settings);
        UImanager.Instance.HideUI(UIType.PauseMenu);
    }

    public void SetCanPause(bool value) {
        canPause = value;
    }

}
