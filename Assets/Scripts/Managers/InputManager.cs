using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Text;
using UnityEditor;
using System.IO;
using System.Collections;

public class InputManager : MonoBehaviour, IObserver {
    public static InputManager Instance { get; private set; }

    public InputActionAsset inputActionAsset;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            StartCoroutine(WaitForGameManagerInitialization());
        }

        playerActionMap = inputActionAsset.FindActionMap("Player");
        uiActionMap = inputActionAsset.FindActionMap("UI");
        playerActionMap.Enable();
    }

    IEnumerator WaitForGameManagerInitialization() {
        while (GameManager.Instance == null) {
            yield return null;
        }
        GameManager.Instance.AddObserver(this);
    }
    public void SwitchToPlayerControls() {
        uiActionMap.Disable();
        playerActionMap.Enable();
    }

    public void SwitchToUIControls() {
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    public void SubscribeToAction(string actionName, Action<InputAction.CallbackContext> callback, bool continuous = false) {
        InputAction[] actions = new InputAction[]
        {
            playerActionMap.FindAction(actionName),
            uiActionMap.FindAction(actionName)
        };

        foreach (var action in actions) {
            if (action != null) {
                action.performed += callback;
                if (continuous) action.canceled += callback;
            }
        }

    }

    public void UnsubscribeFromAction(string actionName, Action<InputAction.CallbackContext> callback, bool continuous = false) {
        InputAction[] actions = new InputAction[]
        {
            playerActionMap.FindAction(actionName),
            uiActionMap.FindAction(actionName)
        };

        foreach (var action in actions) {
            if (action != null) {
                action.performed -= callback;
                if (continuous) action.canceled -= callback;
            }
        }
    }

    public void OnNotify<T>(T data) {
        switch (data) {
            case GameState.Running:
                SwitchToPlayerControls();
                break;
            case GameState.Paused:
                SwitchToUIControls();
                break;
            case GameState.MainMenu:
                SwitchToUIControls();
                break;
            case GameState.Cutscene:
                SwitchToUIControls();
                break;
            case GameState.Dialogue:
                SwitchToUIControls();
                break;

        }
    }
}

/*
 * Subscribing to this input system from external scripts
 * 
 * 
 
    void OnEnable()
    {
        InputManager.Instance.SubscribeToAction("Move", OnMove);
        InputManager.Instance.SubscribeToAction("Jump", OnJump);
    }

    private void OnDisable()
    {
        InputManager.Instance.UnsubscribeFromAction("Move", OnMove);
        InputManager.Instance.UnsubscribeFromAction("Jump", OnJump);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        // Handle move input
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // Handle jump input
    }
*/

/*
 * 
 *  Switching UI / Player externally 
 * 
    InputManager.Instance.SwitchToUIControls();
    InputManager.Instance.SwitchToPlayerControls();
*/