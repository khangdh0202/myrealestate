using System.Collections.Generic;
using UnityEngine;

public class ContructionController : SingletonMonobehaviours<ContructionController>,IDataPersistantce
{
    [SerializeField] private ContructionBuild contructionBuild;
    
    public Material borderContructionMaterial;

    public ContructionBuild ContructionBuild { get => contructionBuild; set => contructionBuild = value; }

    public void LoadData(GameData data)
    {
        Debug.Log("Đang đợi nạp Inventory Item trước khi load contruction!");
        while (!InventoryManager.Instance.loadedInventory) { }

        // Instantiate contruction in data
        foreach (KeyValuePair<string, ContructionData> kvp in data.contructionDictionary)
        {
            string key = kvp.Key;
            ContructionData value = kvp.Value;

            foreach (GameObject item in InventoryManager.Instance.itemLists)
            {
                if (value.itemCode == item.GetComponent<UseItem>().itemDetail.itemCode)
                {
                    GameObject obj = item.GetComponent<UseItem>().ObjectPool.GetObjectFromPool();

                    if (!obj.GetComponent<BoundingBoxBuilding>())
                    {
                        obj.AddComponent<BoundingBoxBuilding>();
                    }

                    if (!obj.GetComponent<ItemInfo>())
                    {
                        obj.AddComponent<ItemInfo>();
                        obj.GetComponent<ItemInfo>().ItemPool = item.GetComponent<UseItem>().ObjectPool;
                    }
                    obj.transform.position = value.position;
                    obj.transform.rotation = Quaternion.Euler(value.rotation);
                    obj.transform.localScale = value.localScale;

                    //obj.layer = LayerMask.NameToLayer("Buidling");
                    obj.tag = Settings.contructionTag;
                

                    obj.GetComponent<BoundingBoxBuilding>().Init();

                    obj.GetComponent<BoundingBoxBuilding>().DrawBorderBuilding(data.settingsData.borderWidth);
                    
                    obj.GetComponent<BoundingBoxBuilding>().OnMap = true;

                    obj.GetComponent<BoundingBoxBuilding>().Id = key;

                };
            }

        }
    }

    public void SaveData(GameData data)
    {
        foreach(string value in PlacementSystem.Instance.keyDeleteObject) 
        {
            if (data.contructionDictionary.ContainsKey(value))
                data.contructionDictionary.Remove(value);
        }
    }
#pragma warning disable

    private void Awake()
    {
        base.Awake();
        ContructionBuild = GetComponent<ContructionBuild>();
    }
#pragma warning restore
}
