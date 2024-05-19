using System.Collections;
using UnityEngine;

public interface IDataPersistantce
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}