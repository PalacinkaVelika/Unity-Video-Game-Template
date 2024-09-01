using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {
    public static UImanager Instance { get; private set; }
    public List<UIElement> uiElements;
    UIType openedUI;
    UIType savedUI;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            HideAllUIs();
        } else {
            Destroy(gameObject);
        }
    }

    public void HideAllUIs() {
        foreach (UIElement uiElement in uiElements) {
            uiElement.uiScript?.Hide();
        }
    }

    public void ShowUI(UIType uiType) {
        var uiElement = uiElements.FirstOrDefault(element => element.uiType == uiType);
        if (uiElement != null) {
            openedUI = uiElement.uiType;
            uiElement.uiScript.Show();
            if(uiElement.defaultSelectedButton != null) EventSystem.current.SetSelectedGameObject(uiElement.defaultSelectedButton.gameObject); // this is Button. i want to select it via code
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
    }

    public void HideUI(UIType uiType) {
        var uiElement = uiElements.FirstOrDefault(element => element.uiType == uiType);
        if (uiElement != null) {
            uiElement.uiScript.Hide();
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
    }

    public GameObject GetCanvasFromUI(UIType uiType) {
        var uiElement = uiElements.FirstOrDefault(element => element.uiType == uiType);
        if (uiElement != null) {
            return uiElement.uiScript.canvas;
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
        return null;
    }

    public void SaveOpenedUI(UIType uiType) {
        savedUI = uiType;
    }
    public void ShowSavedUI() {
        ShowUI(savedUI);
    }
}
public enum UIType {
    MainMenu,
    PauseMenu,
    Dialogue,
    AlertBox,
    HUD,
    Settings,
    LoadingScreen
}

[System.Serializable]
public class UIElement {
    public UIType uiType;
    public UIBehaviour uiScript;
    public Button defaultSelectedButton;
}