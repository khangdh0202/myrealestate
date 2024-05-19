using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ItemList",menuName ="Scriptable Object/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    public List<ItemDetail> itemDetails;

    public int intValue;
    public string stringValue;

    public void SaveToJson()
    {
        string jsonData = JsonUtility.ToJson(this);
        string filePath = Path.Combine(Application.dataPath, "Downloads", "Data_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".json");
        File.WriteAllText(filePath, jsonData);
    }

}
