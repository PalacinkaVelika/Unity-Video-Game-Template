using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UImanager : MonoBehaviour {
    
    public List<UIElement> uiElements;

    public void ShowUI(UIType uiType) {
        var uiElement = uiElements.FirstOrDefault(element => element.uiType == uiType);
        if (uiElement != null) {
            uiElement.uiScript.Show();
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

    public void ToggleUI(UIType uiType) {
        var uiElement = uiElements.FirstOrDefault(element => element.uiType == uiType);
        if (uiElement != null) {
            bool isActive = uiElement.canvas.gameObject.activeSelf;
            uiElement.canvas.gameObject.SetActive(!isActive);
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
    }
}
public enum UIType {
    MainMenu,
    PauseMenu,
    Dialogue,
    AlertBox,
    HUD
}

[System.Serializable]
public class UIElement {
    public UIType uiType;
    public Canvas canvas;
    public Iui uiScript;
}