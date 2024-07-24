using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    public static SaveManager Instance { get; private set; }
    List<ISaveable> saveableObjects;
    string savePath;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            savePath = Application.persistentDataPath + "/savefile.dat";
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

    public void SaveGame() {
        var saveData = new List<Dictionary<string, object>>();
        foreach (var saveable in saveableObjects) {
            saveData.Add(saveable.EncodeData());
        }
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream file = File.Create(savePath)) {
            formatter.Serialize(file, saveData);
        }
    }

    public void LoadGame() {
        if (File.Exists(savePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = File.Open(savePath, FileMode.Open)) {
                var saveData = (List<Dictionary<string, object>>)formatter.Deserialize(file);
                for (int i = 0; i < saveData.Count; i++) {
                    saveableObjects[i].DecodeData(saveData[i]);
                }
            }
        } else {
            Debug.LogWarning("Save file not found!");
        }
    }

    public void UpdateSaveableList() {
        saveableObjects.Clear();
        // Loop through all open scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded) {
                // Loop through all root game objects in the scene
                foreach (GameObject rootGameObject in scene.GetRootGameObjects()) {
                    // Find all ISaveable components in the root and its children
                    ISaveable[] saveables = rootGameObject.GetComponentsInChildren<ISaveable>(true);
                    foreach (ISaveable saveable in saveables) {
                        RegisterSaveable(saveable);
                    }
                }
            }
        }
    }
}
