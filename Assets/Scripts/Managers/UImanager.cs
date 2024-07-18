using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour {
    public List<UIElement> uiElements;


    public void ShowUI(UIType uiType) {
        if (uiDictionary.TryGetValue(uiType, out Canvas canvas)) {
            // canvas.gameObject.SetActive(true); // The UIs will do thisthemselves
            ui.uiScript.Show();
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
    }

    public void HideUI(UIType uiType) {
        if (uiDictionary.TryGetValue(uiType, out Canvas canvas)) {
            canvas.gameObject.SetActive(false);
            ui.uiScript.Hide();
        } else {
            Debug.LogWarning($"UIType {uiType} not found.");
        }
    }

    public void ToggleUI(UIType uiType) {
        if (uiDictionary.TryGetValue(uiType, out Canvas canvas)) {
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
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