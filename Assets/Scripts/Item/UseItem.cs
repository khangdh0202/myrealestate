using System;
using Unity.VisualScripting;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public ItemDetail itemDetail;

    public GameObject B;

    private ObjectPool objectPool;

    public ObjectPool ObjectPool { get => objectPool; set => objectPool = value; }

    public void Init()
    {
        if(!this.GetComponent<ObjectPool>())
        {
            this.gameObject.AddComponent<ObjectPool>();
            ObjectPool = this.GetComponent<ObjectPool>();
            ObjectPool.ObjectPrefab = B;
            ObjectPool.InitializePool();
        }
    }
    public void ClickItem()
    {
        if (PlacementSystem.Instance.mouseIndicator != null)
        {
            PlacementSystem.Instance.CancelItem();
        }

        #region
        // tạo 1 object buildingParent
        GameObject buildingParent = new GameObject("BuildingParent");

        GameObject x = ObjectPool.GetObjectFromPool();

        ContructionController.Instance.ContructionBuild.currentBuidingInMouse = x;

        x.transform.localScale = B.transform.localScale;
        x.transform.localScale = x.transform.localScale/Settings.ratioWorld;
        x.transform.rotation = Quaternion.Euler(Vector3.zero);
        x.transform.position = Vector3.zero;

        GameObject snapArea = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(snapArea.GetComponent<Collider>());

        x.layer = LayerMask.NameToLayer("Building");
        x.tag = Settings.contructionTag;

        if (!x.GetComponent<BoundingBoxBuilding>())
        {
            x.AddComponent<BoundingBoxBuilding>();
        }

        if (!x.GetComponent<ItemInfo>())
        {
            x.AddComponent<ItemInfo>();
            x.GetComponent<ItemInfo>().ItemPool = ObjectPool;
        }

        x.GetComponent<BoundingBoxBuilding>().Init();

        snapArea.AddComponent<ColliderWithBounds>();
        snapArea.transform.parent = buildingParent.transform;

        x.transform.parent = buildingParent.transform;
        snapArea.GetComponent<ColliderWithBounds>().Init(x);

        buildingParent.transform.rotation = Quaternion.Euler(new Vector3());

        PlacementSystem.Instance.mouseIndicator = buildingParent;
        PlacementSystem.Instance.haveObjectInMouseIndicator = true;

        PlacementSystem.Instance.TypeOfMouseIndicator = itemDetail.itemType.ToString();

        #endregion
        PlacementSystem.Instance.CancelSnap();
        Settings.EndRoadExit = true;

        // Chỗ này đáng lẽ là vậy, nhưng do mới làm building thôi nên chỉ lấy được building
        Settings.typeOfCurrentBulld = itemDetail.itemType;
        //Settings.typeOfCurrentBulld = ItemType.Build;
    }
}
