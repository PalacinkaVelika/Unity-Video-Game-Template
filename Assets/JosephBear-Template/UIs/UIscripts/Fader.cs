using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    public static Fader Instance { get; private set; }
    void Awake() {
        if (Instance == null) {
            Instance = this;
            Fade(false, 0f);
        } else {
            Destroy(gameObject);
        }
    }
    public RawImage FadeObj;

    public void Fade(bool fadeIn, float duration = 1f, Color color = default) {
        if (color == default) {
            color = Color.black;
        }
        FadeObj.color = color;
        UtilityUI.Fade(FadeObj.gameObject, fadeIn, duration, false);
    }
}
