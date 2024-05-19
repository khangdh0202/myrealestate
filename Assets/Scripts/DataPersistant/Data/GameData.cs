using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public Vector3 cameraRotation;
    public Vector3 playerPosition;
    public string timeSave;
    public bool autoSaving;
    public float sunRotation;

    public SerializableDictionary<string, ContructionData> contructionDictionary;
    public SerializableDictionary<string, RoadData> roadDictionary;
    public SettingsData settingsData;

    public GameData() 
    { 
        this.cameraRotation = Vector3.zero;
        this.playerPosition = new Vector3(0,5,0);
        this.contructionDictionary = new SerializableDictionary<string, ContructionData>();
        this.roadDictionary = new SerializableDictionary<string, RoadData>();
        this.settingsData = new SettingsData();
        this.autoSaving = true;
        this.sunRotation = 40f;
    }
}
