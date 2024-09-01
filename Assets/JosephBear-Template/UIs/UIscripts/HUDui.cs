using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDui : UIBehaviour {
    public static HUDui Instance { get; private set; }
    public TMP_Text text_score;


    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    public override void Hide() {
        canvas.SetActive(false);
    }

    public override void Show() {
        canvas.SetActive(true);
    }

    public void UpdateScore(int score) {
        text_score.text = score.ToString();
    }
}
