using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    public static SaveManager Instance { get; private set; }
    private List<ISaveable> saveableObjects;
    private GameData loadedSaveData;
    private SettingsData loadedSettingsData;
    private string savePath;
    private string settingsPath;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            savePath = Application.persistentDataPath + "/savefile.dat";
            settingsPath = Application.persistentDataPath + "/settingsfile.dat";
            saveableObjects = new List<ISaveable>();
        }
    }

    public void RegisterSaveable(ISaveable saveable) {
        if (!saveableObjects.Contains(saveable)) {
            saveableObjects.Add(saveable);
        }
    }

    public void UnregisterSaveable(ISaveable saveable) {
        if (saveableObjects.Contains(saveable)) {
            saveableObjects.Remove(saveable);
        }
    }

    public void ClearSaveData() {
        GameData saveData = new GameData();
        loadedSaveData = saveData;
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream file = File.Create(savePath)) {
            formatter.Serialize(file, saveData);
        }
    }

    public void SaveGame() {
        GameData saveData = new GameData();
        foreach (var saveable in saveableObjects) {
            saveable.SaveData(saveData);
        }
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream file = File.Create(savePath)) {
            formatter.Serialize(file, saveData);
        }
    }

    /// <summary>
    /// Tells all ISaveables that are loaded in SaveManager to Load data from GameData
    /// LoadGameData -> UpdateSaveableList -> LoadGame
    /// </summary>
    public void LoadGame() {
        foreach (var saveable in saveableObjects) {
            saveable.LoadData(loadedSaveData);
        }
    }

    public async Task LoadGameDataAsync() {
        if (File.Exists(savePath)) {
            try {
                loadedSaveData = await Task.Run(() => {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream file = File.Open(savePath, FileMode.Open)) {
                        return (GameData)formatter.Deserialize(file);
                    }
                });
            } catch (Exception e) {
                Debug.LogError("Failed to load save file: " + e.Message);
            }
        } else {
            Debug.LogWarning("Save file not found!");
        }
    }

    public GameData GetLoadedData() {
        return loadedSaveData;
    }
    public SettingsData GetLoadedSettingsData() {
        return loadedSettingsData;
    }

    public void UpdateSaveableList() {
        saveableObjects.Clear();
        // Loop through all open scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded) {
                foreach (GameObject rootGameObject in scene.GetRootGameObjects()) {
                    ISaveable[] saveables = rootGameObject.GetComponentsInChildren<ISaveable>(true);
                    foreach (ISaveable saveable in saveables) {
                        RegisterSaveable(saveable);
                    }
                }
            }
        }
    }

    public void SaveSettings(SettingsData settingsData) {
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = File.Create(GetSettingsPath())) {
                formatter.Serialize(file, settingsData);
            }
        } catch (Exception e) {
            Debug.LogError("Saving settings failed: " + e.Message);
        }
    }

    public void LoadSettings() {
        if (File.Exists(GetSettingsPath())) {
            try {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream file = File.Open(GetSettingsPath(), FileMode.Open)) {
                    SettingsData settingsData = (SettingsData)formatter.Deserialize(file);
                    SettingsManager.Instance.ApplySettings(settingsData);
                }
            } catch (Exception e) {
                Debug.LogError("Failed to load settings: " + e.Message);
            }
        } else {
            Debug.LogWarning("Settings file not found!");
        }
    }
        
    string GetSettingsPath() {
        return settingsPath;
    }
}
