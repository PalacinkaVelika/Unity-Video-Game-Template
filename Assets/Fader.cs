using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public RawImage FadeObj;

    private void Awake() {
        Fade(false, 0f);
    }

    public void Fade(bool fadeIn, float duration = 1f, Color color = default) {
        if (color == default) {
            color = Color.black;
        }
        FadeObj.color = color;
        UtilityUI.Fade(FadeObj.gameObject, fadeIn, duration, false);
    }
}
