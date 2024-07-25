using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable {
    void SaveData(GameData data);
    void LoadData(GameData data);
}
/*
 * 
    public int playerHealth;
    public Vector3 playerPosition;
    // Encode data to a dictionary
    public Dictionary<string, object> EncodeData() {
        var data = new Dictionary<string, object>();
        data["playerHealth"] = playerHealth;
        data["playerPosition"] = new float[] { playerPosition.x, playerPosition.y, playerPosition.z };
        return data;
    }
    // Decode data from a dictionary
    public void DecodeData(Dictionary<string, object> data) {
        if (data.ContainsKey("playerHealth")) {
            playerHealth = (int)data["playerHealth"];
        } else {
            print("cant load player health");
        }
        if (data.ContainsKey("playerPosition"))
        {
            float[] pos = (float[])data["playerPosition"];
            playerPosition = new Vector3(pos[0], pos[1], pos[2]);
        } else {
            print("cant load player position");
        }
    }
 *
 *
 */