using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager: SingletonMonobehaviours<InventoryManager>
{

    private List<ItemDetail> itemDetailDictionary;

    [Header("Scriptable Object config")]
    [SerializeField] private SO_ItemList itemListSO;
    [SerializeField] List<GameObject> constructionList;
    [SerializeField] List<GameObject> vegetationList;
    [SerializeField] List<GameObject> propsList;

    // prefab item
    public GameObject inventoryItem;
    public bool loadedInventory = false;

    //
    public GameObject innventoryScroll;
    ScrollRect scrollRect;

    public List<GameObject> itemLists = new List<GameObject>();

    // View, kho chưa các item khi nạp vào 
    public GameObject buildingContentView;
    public GameObject propsContentView;
    public GameObject vegetationContentView;
    public GameObject bannerContentView;
    public GameObject showAllContentView;

    // Canvas UI Inventory
    public GameObject canvasUiInventory;


    public List<ItemDetail> ItemDetailDictionary { get => itemDetailDictionary; }

    protected override void Awake()
    {
        base.Awake();

        // Dọn SO nếu như có value cũ 
        itemListSO.itemDetails.Clear();

        int code = 10000;
        // Nạp construction
        foreach (GameObject obj in constructionList)
        {
            itemListSO.itemDetails.Add(new ItemDetail((code+1).ToString(), obj, ItemType.Build, obj.name));
            code++;
        }
        // Nạp Scenary
        foreach (GameObject obj in vegetationList)
        {
            itemListSO.itemDetails.Add(new ItemDetail((code + 1).ToString(), obj, ItemType.Scenary, obj.name));
            code++;
        }
        // Nạp props
        foreach (GameObject obj in propsList)
        {
            itemListSO.itemDetails.Add(new ItemDetail((code + 1).ToString(), obj, ItemType.Prop, obj.name));
            code++;
        }

        CreateItemDetailDictionary();
        ListItem();

        /*string jsonData = JsonUtility.ToJson(itemListSO);
        File.WriteAllText(Application.dataPath + "/data.json", jsonData);*/
    }

    private void Start()
    {
        canvasUiInventory.SetActive(false);
        buildingContentView.SetActive(false);
        propsContentView.SetActive(false);
        vegetationContentView.SetActive(false);
        bannerContentView.SetActive(false);
        showAllContentView.SetActive(true);
        scrollRect = innventoryScroll.GetComponent<ScrollRect>();
        scrollRect.content = showAllContentView.GetComponent<RectTransform>();
    }
    /// <summary>
    /// tạo list item 
    /// </summary>
    private void CreateItemDetailDictionary()
    {
        itemDetailDictionary = new List<ItemDetail>();

        foreach (ItemDetail itemDetail in itemListSO.itemDetails)
        {
            ItemDetailDictionary.Add(itemDetail);
        }
    }


    /// <summary>
    /// Nạp toàn bộ item trong itemDetailDictinary
    /// </summary>
    public void ListItem()
    {
        // Chắc chắn trước khi chạy sẽ không có item nào trong UI;

        DesTroy();

        foreach (var item in ItemDetailDictionary)
        {
            if (item.itemType == ItemType.Build)
            {
                LoadItemInUI(item, buildingContentView.transform);
            }
            else if (item.itemType == ItemType.Prop)
            {
                LoadItemInUI(item, propsContentView.transform);
            }
            else if (item.itemType == ItemType.Banner)
            {
                LoadItemInUI(item, bannerContentView.transform);
            }
            else if (item.itemType == ItemType.Scenary)
            {
                LoadItemInUI(item, vegetationContentView.transform);
            }
        }

        loadedInventory = true;
    }
    private void DesTroy()
    {
        foreach (Transform item in buildingContentView.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in propsContentView.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in bannerContentView.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in vegetationContentView.transform)
        {
            Destroy(item.gameObject);
        }
    }
    private void LoadItemInUI(ItemDetail item, Transform itemContent)
    {
        GameObject obj = Instantiate(inventoryItem, itemContent);
        itemLists.Add(obj);
        var itemTitle = obj.transform.Find("ItemName").GetComponent<Text>();
        var itemImage = obj.transform.Find("Image").GetComponent<Image>();
        UseItem itemUse = obj.GetComponent<UseItem>();

        itemTitle.text = item.itemTitle;
        itemUse.itemDetail = item;
        itemImage.sprite = item.itemImage;
        itemUse.B = item.itemModel;

        itemUse.Init();
    }

    bool firstOpenInventory = true;
    public void FirstTimeActiveInventory()
    {
        if(firstOpenInventory)
        {
            ShowAllTabActive();
            firstOpenInventory = false;
        }
    }
    
    public void BuildingTabActive()
    {
        scrollRect.content = buildingContentView.GetComponent<RectTransform>();

        foreach (GameObject obj in itemLists) 
        {
            if (obj.GetComponent<UseItem>().itemDetail.itemType == ItemType.Build)
                obj.transform.SetParent(buildingContentView.transform, false);
        }

        buildingContentView.SetActive(true);
        propsContentView.SetActive(false);
        vegetationContentView.SetActive(false);
        bannerContentView.SetActive(false);
        showAllContentView.SetActive(false);
    }
    public void PropsTabActive()
    {
        scrollRect.content = propsContentView.GetComponent<RectTransform>();

        foreach (GameObject obj in itemLists)
        {
            if (obj.GetComponent<UseItem>().itemDetail.itemType == ItemType.Prop)
                obj.transform.SetParent(propsContentView.transform, false);
        }

        buildingContentView.SetActive(false);
        propsContentView.SetActive(true);
        vegetationContentView.SetActive(false);
        bannerContentView.SetActive(false);
        showAllContentView.SetActive(false);
    }
    public void VegetationTabActive()
    {
        scrollRect.content = vegetationContentView.GetComponent<RectTransform>();

        foreach (GameObject obj in itemLists)
        {
            if (obj.GetComponent<UseItem>().itemDetail.itemType == ItemType.Scenary)
                obj.transform.SetParent(vegetationContentView.transform, false);
        }

        buildingContentView.SetActive(false);
        propsContentView.SetActive(false);
        vegetationContentView.SetActive(true);
        bannerContentView.SetActive(false);
        showAllContentView.SetActive(false);
    }
    public void BannerTabActive()
    {
        scrollRect.content = bannerContentView.GetComponent<RectTransform>();

        foreach (GameObject obj in itemLists)
        {
            if (obj.GetComponent<UseItem>().itemDetail.itemType == ItemType.Banner)
                obj.transform.SetParent(bannerContentView.transform, false);
        }

        buildingContentView.SetActive(false);
        propsContentView.SetActive(false);
        vegetationContentView.SetActive(false);
        bannerContentView.SetActive(true);
        showAllContentView.SetActive(false);
    }
    public void ShowAllTabActive()
    {
        scrollRect.content = showAllContentView.GetComponent<RectTransform>();

        foreach (GameObject obj in itemLists)
        {
            obj.transform.SetParent(showAllContentView.transform, false);
        }

        buildingContentView.SetActive(false);
        propsContentView.SetActive(false);
        vegetationContentView.SetActive(false);
        bannerContentView.SetActive(false);
        showAllContentView.SetActive(true);
    }

}
