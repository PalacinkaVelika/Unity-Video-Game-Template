using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour, IObserver {

    public float moveSpeed = 5f;
    Rigidbody rb;
    Vector2 moveInput;
    bool canMove = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        Move();
    }
    void OnMove(InputAction.CallbackContext context) {
        if (context.performed) {
            moveInput = context.ReadValue<Vector2>();
        } else if (context.canceled) {
            moveInput = Vector2.zero;
        }
    }

    void OnJump(InputAction.CallbackContext context) {
        if (context.performed) {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    void Move() {
        if (canMove) {
            Vector3 movement = new Vector3(moveInput.x, 0, 0) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    void OnEnable() {
        GameManager.Instance?.AddObserver(this);
        InputManager.Instance?.SubscribeToAction("Move", OnMove, true);
        InputManager.Instance?.SubscribeToAction("Jump", OnJump);
    }

    void OnDisable() {
        GameManager.Instance?.RemoveObserver(this);
        InputManager.Instance?.UnsubscribeFromAction("Move", OnMove, true);
        InputManager.Instance?.UnsubscribeFromAction("Jump", OnJump);
    }

    public void OnNotify<T>(T data) {
        switch (data) {
            case GameState.Running:
                canMove = true;
                break;
            case GameState.Paused:
                canMove = false;
                break;
            case GameState.MainMenu:
                canMove = false;
                break;
            case GameState.Cutscene:
                canMove = false;
                break;
            case GameState.Dialogue:
                canMove = false;
                break;
        }
    }
}
