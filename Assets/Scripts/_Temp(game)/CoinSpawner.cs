using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour, ISaveable {
    
    int score = 0;

    public GameObject Coin;
    public float minX = 0f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 10f;
    HUDui hud;

    void OnEnable() {
        hud = HUDui.Instance;
        UpdateHUD();
    //    SaveManager.Instance.RegisterSaveable(this);
    }

    public void CoinCollected() {
        score++;
        AudioManager.Instance.PlaySound(SoundType.manScream);
        AudioManager.Instance.PlaySound(SoundType.neckCrunch);
        UpdateHUD();
        ScreenEffectManager.Instance.ScreenShakeImpulse(2f,2f,.1f);
        SpawnNewCoin();
        // saving after every coin cuz fuck you
        SaveManager.Instance.SaveGame(); // This is definetly wrong and i will get arrested for writing into a file gazillion times a second
    }

    void SpawnNewCoin() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 newCoinPosition = new Vector3(randomX, randomY, 0f);
        // The correct way to instantiate objects and have them stay in the scene
        SceneLoadingManager.Instance.InstantiateObjectInScene(Coin, newCoinPosition, SceneType.GameplayScene);
    } 

    void UpdateHUD() {
        hud.UpdateScore(score);
    }

    public void SaveData(GameData data) {
        data.playerScore = score;
    }

    public void LoadData(GameData data) {
        score = data.playerScore;
        UpdateHUD();
    }

}
