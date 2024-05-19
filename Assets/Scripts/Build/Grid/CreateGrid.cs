/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngineInternal;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Drawing;

public class CreateGrid : SingletonMonobehaviours<CreateGrid>
{
    [SerializeField]
    private GameObject gridPrefab;

    private ObjectPool objectPool;

    public GameObject canvasUIEditGridMenu;

    public TMP_InputField xScale;
    public TMP_InputField yScale;
    public TMP_InputField xSize;
    public TMP_InputField ySize;

    [Tooltip("Danh sach cac grid parent duoc dat tren map")]
    public List<GameObject> gridList; 

    private void Start()
    {
        canvasUIEditGridMenu.SetActive(false);
        objectPool = gameObject.GetComponent<ObjectPool>();
    }

    private void Update()
    {
        OnSubmitXSize(xSize.text);
        OnSubmitYSize(ySize.text);
        //OnSubmitXScale(xScale.text);
        //OnSubmitZScale(yScale.text);
    }

    public void CreateAGrid()
    {
        if (UISystem.Instance.UICanvasGameobject.active == true)
            UISystem.Instance.UICanvasGameobject.SetActive(false);

        // tạo grid parent 
        GameObject parentObject = new GameObject("ParentGridObject");
        GameObject grid = objectPool.GetObjectFromPool();
        grid.transform.parent = parentObject.transform;
        grid.transform.position = parentObject.transform.position;
        parentObject.tag = UnityEditorInternal.InternalEditorUtility.tags[12];

        Material objectMaterial;

        objectMaterial = grid.gameObject.GetComponent<MeshRenderer>().material;
        UnityEngine.Color colorMaterial = objectMaterial.GetColor("_Color");
        colorMaterial = UnityEngine.Color.white;
        objectMaterial.SetColor("_Color", colorMaterial);

        PlacementSystem.Instance.cellIndicator.SetActive(true);

        PlacementSystem.Instance.mouseIndicator = parentObject;
        EditGridMenuActive();
    }

    public void EditGridMenuActive()
    {
        //Debug.Log("Day la ben ngoai Edit Grid Menu");
        if (Settings.isOpenEditGrid == false)
        {
            //Debug.Log("Day la ben trong Edit Grid Menu");
            // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
            Vector3 headsetPosition = Camera.main.transform.position;
            Vector3 directionVec = Camera.main.transform.forward;

            // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
            canvasUIEditGridMenu.transform.position = headsetPosition + directionVec * 3f; // 2f là khoảng cách 

            canvasUIEditGridMenu.transform.rotation = Quaternion.LookRotation(directionVec);

            Settings.isOpenEditGrid = true;
            canvasUIEditGridMenu.SetActive(true);

            InputEditGridMenu();
        }
    }

    public void CloseEditGridMenu()
    {
        if(Settings.isOpenEditGrid == true)
        {
            PlacementSystem.Instance.GridDefault();
            NonNativeKeyboard.Instance.Close();
            PlacementSystem.Instance.CancelItem(); // Xóa đối tượng build hiện đang làm việc nếu có
            Settings.isOpenEditGrid = false;
            canvasUIEditGridMenu.SetActive(false);
            PlacementSystem.Instance.cellIndicator.SetActive(false);
        }
    }

    private Material GetMaterialOfGrid()
    {
        Material grid = PlacementSystem.Instance.mouseIndicator.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        return grid;
    }

    /// <summary>
    /// đổ dữ liệu của Grid vào UI 
    /// </summary>
    public void InputEditGridMenu()
    {
        //Vector3 scale = PlacementSystem.Instance.mouseIndicator.gameObject.transform.localScale;
        Vector3 scale = PlacementSystem.Instance.mouseIndicator.transform.GetChild(0).gameObject.transform.localScale;

        // Trong Prefab nó properties của Unity, thuộc tính là 'Size', nhưng để lấy ra thì phải thêm '_' vào
        Vector2 size = GetMaterialOfGrid().GetVector("_Size");

        xScale.text = scale.x.ToString();
        yScale.text = scale.y.ToString();
        xSize.text = size.x.ToString();
        ySize.text = size.y.ToString();

    }
    public void OnSubmitXScale(string value)
    {
        //int newValue = int.Parse(value); // Chuyển đổi giá trị sang kiểu dữ liệu mong muốn (ở đây là float)




        *//*if (Mathf.Abs(newValue % 2) < 0.01f)
        {
            if (!Settings.evenNumberXScale)
            {
                Settings.evenNumberXScale = true;
            }
        }

        // Cập nhật giá trị vào đối tượng của bạn ở đây
        Vector3 newScale = PlacementSystem.Instance.mouseIndicator.gameObject.transform.localScale;

        newScale.x = newValue;

        // Gán giá trị scale mới vào đối tượng của bạn
        PlacementSystem.Instance.mouseIndicator.gameObject.transform.localScale = newScale;

        
        Vector3 newScaleCell = PlacementSystem.Instance.cellIndicator.gameObject.transform.localScale;

        newScaleCell.x = newValue;

        // Gán giá trị scale mới vào đối tượng của bạn
        PlacementSystem.Instance.cellIndicator.gameObject.transform.localScale = newScale;*//*
        
    }
    public void OnSubmitZScale(string value)
    {
        int newValue = int.Parse(value); // Chuyển đổi giá trị sang kiểu dữ liệu mong muốn (ở đây là float)

        if (Mathf.Abs(newValue % 2) < 0.01f)
        {
            if (!Settings.evenNumberYScale)
            {
                Settings.evenNumberYScale = true;
            }
        }

        // Cập nhật giá trị vào đối tượng của bạn ở đây
        Vector3 newScale = PlacementSystem.Instance.mouseIndicator.gameObject.transform.localScale;

        newScale.z = newValue;

        // Gán giá trị scale mới vào đối tượng của bạn
        PlacementSystem.Instance.mouseIndicator.gameObject.transform.localScale = newScale;

        Vector3 newScaleCell = PlacementSystem.Instance.cellIndicator.gameObject.transform.localScale;

        newScaleCell.z = newValue;

        // Gán giá trị scale mới vào đối tượng của bạn
        PlacementSystem.Instance.cellIndicator.gameObject.transform.localScale = newScale;
    }

    /// <summary>
    /// Từ grid mặc định làm nhỏ nó bao nhiêu phần
    /// </summary>
    /// <param name="value"></param>

    public void OnSubmitXSize(string value)
    {
        // giá trị scale của grid trên editor 
        float newValue = float.Parse(value); // Chuyển đổi giá trị sang kiểu dữ liệu mong muốn (ở đây là float)

        for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
        {
            //PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).Find(Settings.gridName).GetComponent<GridController>().OnMap = true;

            Vector3 newScale = PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).localScale;

            //Debug.Log($"Newvalue = {newValue}");

            //PlacementSystem.Instance.mouseIndicator.transform.localScale = new Vector3(newValue, newScale.y, newScale.z);
            PlacementSystem.Instance.UpdateScaleMouseIndicator(PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject, newValue, newScale.y, newScale.z);
            //Player.Instance.UpdateScale?.Invoke(newValue, newScale.y, newScale.z);

            Vector2 size = GetMaterialOfGrid().GetVector("_Size");

            // Giá trị kích thước material để scale bằng với kích thước grid hiện tại 
            size.x = 1f / newValue;

            // Gán giá trị mới cho Material
            GetMaterialOfGrid().SetVector("_Size", size);
            PlacementSystem.Instance.GetMaterialOfCellIndicatior().SetVector("_Size", size);
        }

    }
    public void OnSubmitYSize(string value)
    {
        float newValue = float.Parse(value); // Chuyển đổi giá trị sang kiểu dữ liệu mong muốn (ở đây là float)

        for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
        {
            //PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).Find(Settings.gridName).GetComponent<GridController>().OnMap = true;

            Vector3 newScale = PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).localScale;

            //Debug.Log($"Newvalue = {newValue}");

            PlacementSystem.Instance.UpdateScaleMouseIndicator(PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject, newScale.x, newScale.y, newValue);
            //Player.Instance.UpdateScale?.Invoke(newScale.x, newScale.y, newValue);
            //PlacementSystem.Instance.mouseIndicator.transform.localScale = new Vector3(newScale.x, newScale.y, newValue);

            Vector2 size = GetMaterialOfGrid().GetVector("_Size");

            // Giá trị kích thước material để scale bằng với kích thước grid hiện tại 
            size.y = 1f / newValue;

            // Gán giá trị mới cho Material
            GetMaterialOfGrid().SetVector("_Size", size);
            PlacementSystem.Instance.GetMaterialOfCellIndicatior().SetVector("_Size", size);
        }
    }
}
*/