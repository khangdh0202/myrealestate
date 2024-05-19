using UnityEditor;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[System.Serializable]
public class ItemDetail 
{
    public string itemCode;
    public ItemType itemType;
    public Sprite itemImage;
    public string itemTitle;
    public GameObject itemModel;

    public ItemDetail(string id,GameObject itemModel, ItemType itemType, string name)
    {
        if(id == null)
            this.itemCode = System.Guid.NewGuid().ToString();
        else 
            this.itemCode = id;
        this.itemType = itemType;
        //this.itemImage = itemImage;
        this.itemTitle = name;
        this.itemModel = itemModel;

        // Nếu tệp tồn tại, Gán null
        this.itemImage = Resources.Load<Sprite>("Sprites/" + name);
    }
}
