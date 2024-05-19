using UnityEngine;

[System.Serializable]
public class ContructionData
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 localScale;
    public string itemCode;

    public ContructionData(GameObject data) {
        position = data.transform.position;

        rotation = data.transform.rotation.eulerAngles;

        localScale = data.transform.localScale;

        itemCode = "null";

        if(data.GetComponent<ItemInfo>())
            if(data.GetComponent<ItemInfo>().ItemPool.gameObject.GetComponent<UseItem>())
                itemCode = data.GetComponent<ItemInfo>().ItemPool.gameObject.GetComponent<UseItem>().itemDetail.itemCode;
    }
}
