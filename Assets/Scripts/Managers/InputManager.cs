using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Text;
using UnityEditor;
using System.IO;

public class InputManager : MonoBehaviour {
    public static InputManager Instance { get; private set; }

    public InputActionAsset inputActionAsset;
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
        playerActionMap = inputActionAsset.FindActionMap("Player");
        uiActionMap = inputActionAsset.FindActionMap("UI");
        playerActionMap.Enable();
    }

    public void SwitchToPlayerControls() {
        uiActionMap.Disable();
        playerActionMap.Enable();
    }

    public void SwitchToUIControls() {
        playerActionMap.Disable();
        uiActionMap.Enable();
    }

    public void SubscribeToAction(string actionName, Action<InputAction.CallbackContext> callback) {
        InputAction action = playerActionMap.FindAction(actionName) ?? uiActionMap.FindAction(actionName);
        if (action != null) {
            action.performed += callback;
        }
    }

    public void UnsubscribeFromAction(string actionName, Action<InputAction.CallbackContext> callback) {
        InputAction action = playerActionMap.FindAction(actionName) ?? uiActionMap.FindAction(actionName);
        if (action != null) {
            action.performed -= callback;
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