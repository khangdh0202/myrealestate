using System;
using UnityEngine;
using KhangLibrary;
using Unity.VisualScripting;
using UnityEditor;

public class BoundingBoxBuilding : MonoBehaviour, IDataPersistantce
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        Id= System.Guid.NewGuid().ToString();
    }


    public void LoadData(GameData data)
    {

    }

    public void SaveData(GameData data)
    {
        if (data.contructionDictionary.ContainsKey(Id))
            data.contructionDictionary.Remove(Id);

        if (OnMap)
            data.contructionDictionary.Add(Id, new ContructionData(this.gameObject));
    }

    public BoxCollider buildingCollider;

    [Tooltip("Chứa thông tin bound của object")] private Bounds objectBound;

    private bool _OnMap;

    private Vector3[] pointBottomBoxCollider = new Vector3[4];

    private GameObject[] pointOfBuildingObject = new GameObject[4];

    public Bounds ObjectBound { get => objectBound; set => objectBound = value; }
    public bool OnMap { get => _OnMap; set => _OnMap = value; }
    public Vector3[] PointBottomBoxCollider { get => pointBottomBoxCollider; set => pointBottomBoxCollider = value; }
    public GameObject[] PointOfBuildingObject { get => pointOfBuildingObject; set => pointOfBuildingObject = value; }
    public string Id { get => id; set => id = value; }

    private void Awake()
    {
        GenerateGuid();
    }

    private void Update()
    {

        if (OnMap)
            return;

        try {
            if (buildingCollider.bounds != null)
            {
                ObjectBound = buildingCollider.bounds;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        if (!OnMap)
        {
            if (buildingCollider != null)
            {
                // Lấy 4 điểm dưới của BoxCollider
                PointBottomBoxCollider = GetBottomPoints(buildingCollider);
                for (int i = 0; i < 4; i++)
                {
                    PointOfBuildingObject[i].transform.position = PointBottomBoxCollider[i];
                }
            }
            DrawBorderBuilding(SettingManager.Instance.borderContructionWidth);
        }

        for (int i = 0; i < 4; i++)
        {
            PointOfBuildingObject[i].transform.position = PointBottomBoxCollider[i];
        }
    }

    private Vector3 point1, point2, point3;
    private Vector3 diemTrenDuong;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Vẽ bounding box bằng hàm OnDrawGizmos
        Gizmos.DrawWireCube(ObjectBound.center, ObjectBound.size);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(PointOfBuildingObject[0].transform.position, 0.05f);
        Gizmos.DrawSphere(PointOfBuildingObject[1].transform.position, 0.05f);
        Gizmos.DrawSphere(PointOfBuildingObject[2].transform.position, 0.05f);
        Gizmos.DrawSphere(PointOfBuildingObject[3].transform.position, 0.05f);


        Gizmos.color = Color.green;
        Gizmos.DrawSphere(point1, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point2, 0.2f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(point3, 0.2f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(diemTrenDuong, 0.2f);
        Gizmos.DrawLine(point3, diemTrenDuong);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(point1, point2);
    }
    public void Init()
    {
        while (gameObject.GetComponent<Collider>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<Collider>());
        }

        // Khởi tạo biến để lưu trữ giới hạn tổng cộng của tất cả các mesh
        Bounds combinedBounds = new Bounds();

        // Lấy ra MeshFilter của gameObject cha nếu có
        MeshFilter parentMeshFilter = gameObject.GetComponent<MeshFilter>();
        if (parentMeshFilter != null && parentMeshFilter.sharedMesh != null)
        {
            // Lấy ra giới hạn của mesh của gameObject cha và mở rộng giới hạn tổng cộng
            combinedBounds.Encapsulate(parentMeshFilter.sharedMesh.bounds);
        }

        // Lặp qua tất cả các children của gameObject
        for (int x = 0; x < gameObject.transform.childCount; x++)
        {
            // Lấy ra child tại vị trí x
            Transform child = gameObject.transform.GetChild(x);

            // Kiểm tra xem child có chứa MeshFilter không
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                // Lấy ra giới hạn của mesh và mở rộng giới hạn tổng cộng
                combinedBounds.Encapsulate(meshFilter.sharedMesh.bounds);
            }
        }

        // Tạo một BoxCollider dựa trên giới hạn tổng cộng của mesh của gameObject cha và mesh của tất cả các con
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        // Lấy vị trí ban đầu của gameObject cha
        Vector3 initialPosition = gameObject.transform.position;

        // Gán giới hạn tổng cộng cho BoxCollider của gameObject cha
        boxCollider.center = combinedBounds.center - initialPosition;
        boxCollider.center = new Vector3(0, boxCollider.center.y, 0);
        boxCollider.size = combinedBounds.size;

        for (int x = 0; x < gameObject.transform.childCount; x++){
            if(gameObject.transform.GetChild(x).name == "New Game Object")
            {
                Destroy(gameObject.transform.GetChild(x).gameObject);
                continue;
            }
        }

        // add collider 
        //gameObject.AddComponent<BoxCollider>();

        buildingCollider = this.GetComponent<BoxCollider>();
        buildingCollider.isTrigger = true;
        OnMap = false;

        PointBottomBoxCollider = GetBottomPoints(buildingCollider);

        for (int i = 0; i < 4; i++)
        {
            PointOfBuildingObject[i] = new GameObject();
            PointOfBuildingObject[i].transform.parent = transform;
            BoxCollider collider = PointOfBuildingObject[i].AddComponent<BoxCollider>();

            // Lấy kích thước của collider object hiện tại
            Bounds currentColliderBounds = GetComponent<BoxCollider>().bounds;

            // Lấy kích thước theo trục x và z và giảm đi 10%
            /*float newColliderSizeX = currentColliderBounds.size.x * 0.1f;
            float newColliderSizeY = currentColliderBounds.size.y * 0.1f;
            float newColliderSizeZ = currentColliderBounds.size.z * 0.1f;*/
            float newColliderSizeX = 0.2f;
            float newColliderSizeY = 0.2f;
            float newColliderSizeZ = 0.2f;

            // Đặt kích thước mới cho box collider
            collider.size = new Vector3(newColliderSizeX, newColliderSizeY, newColliderSizeZ);
            PointOfBuildingObject[i].transform.position = PointBottomBoxCollider[i];
        }

        if (buildingCollider != null)
        {
            ObjectBound = buildingCollider.bounds;
            return;
        }
        else
        {
            Debug.LogError("Collider not found on the object.");
        }

    }

    bool ArePointsFormingRectangle(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        // Kiểm tra chiều dài các cạnh đối diện
        float edge01 = Vector3.Distance(point0, point1);
        float edge23 = Vector3.Distance(point2, point3);

        float edge12 = Vector3.Distance(point1, point2);
        float edge30 = Vector3.Distance(point3, point0);

        // Điều kiện để là hình chữ nhật
        bool oppositeEdgesEqual = Mathf.Approximately(edge01, edge23) && Mathf.Approximately(edge12, edge30);

        return oppositeEdgesEqual;
    }

    Vector3[] GetBottomPoints(BoxCollider collider)
    {
        Vector3[] bottomPoints = new Vector3[4];

        // Lấy tâm của BoxCollider trong không gian thế giới
        Vector3 center = collider.center;

        // Lấy kích thước của BoxCollider
        Vector3 size = collider.size;

        // Tính toán các điểm dưới dựa trên ma trận quay và vị trí của BoxCollider
        bottomPoints[0] = transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z) * 0.5f);
        bottomPoints[1] = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
        bottomPoints[2] = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
        bottomPoints[3] = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);

        bottomPoints[0].y = this.transform.position.y;
        bottomPoints[1].y = this.transform.position.y;
        bottomPoints[2].y = this.transform.position.y;
        bottomPoints[3].y = this.transform.position.y;

        return bottomPoints;
    }

    Vector3 singlePoint = new Vector3();

    /// <summary>
    /// cho vào 1 điểm, tìm ra cạnh gần nhất của điểm đó 
    /// </summary>
    /// <param name="singlePoint"></param>
    /// <returns></returns>
    public Vector3[] GetLineNearest(Vector3 singlePoint)
    {
        this.singlePoint = singlePoint;
        Vector3[] point = new Vector3[2];
        float min = float.MaxValue;

        for (int i=0;i< PointBottomBoxCollider.Length;i++)
        {
            float line;
            line = Vector3.Distance(LineIntersection3D.GetPerpendicularPoint(PointBottomBoxCollider[i], PointBottomBoxCollider[(i+1)==PointBottomBoxCollider.Length?0: i + 1], singlePoint),singlePoint);
            if (min > line)
            {
                min = line;
                point[0] = PointBottomBoxCollider[i];
                point[1] = PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1];
            }
        }

        return point;
    }
    /// <summary>
    /// trả về đoạn thẳng gần nhất và song song với đoạn thẳng truyền vào
    /// </summary>
    /// <param name="startLine"></param>
    /// <param name="endLine"></param>
    /// <returns></returns>
    public Vector3[] GetLineParallelAndNearestWithALine(Vector3 startPoint, Vector3 endPoint, Vector3 singlePoint)
    {
        Vector3[] point = new Vector3[2];
        float minDistance = float.MaxValue;

        for (int i = 0; i < PointBottomBoxCollider.Length; i++)
        {

            if (LineIntersection3D.ArePerpendicular(singlePoint, LineIntersection3D.GetPerpendicularPoint( PointBottomBoxCollider[i], PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1], singlePoint), PointBottomBoxCollider[i], PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1]))
            {
                //Vector3 point1 = LineIntersection3D.GetPerpendicularPoint(PointBottomBoxCollider[i], PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1], singlePoint);
                if (LineIntersection3D.ArePerpendicular(LineIntersection3D.GetPerpendicularPoint(PointBottomBoxCollider[i], PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1], singlePoint), singlePoint,startPoint,endPoint))
                {
                    point3 = LineIntersection3D.GetPerpendicularPoint(PointBottomBoxCollider[i], PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1], singlePoint);

                    float distance = Vector3.Distance(point3, singlePoint);

                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        point1 = PointBottomBoxCollider[i];
                        point2 = PointBottomBoxCollider[(i + 1) == PointBottomBoxCollider.Length ? 0 : i + 1];
                        point3 = LineIntersection3D.GetPerpendicularPoint(point1, point2, singlePoint);
                    }
                }
            }
        }

        return point;
    }
    /// <summary>
    /// Draw a border in map for user can see
    /// </summary>
    public void DrawBorderBuilding(float size)
    {
        if (!SettingManager.Instance.borderContructionView)
        {
            if (transform.Find("Border"))
            {
                transform.Find("Border").gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            if (transform.Find("Border"))
            {
                transform.Find("Border").gameObject.SetActive(true);
            }
        }

        if (!transform.Find("Border"))
        {
            GameObject obj = new GameObject("Border", typeof(LineRenderer));
            obj.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
            obj.transform.parent = transform;
        }

        if(!transform.Find("Border").GetComponent<LineRenderer>())
            transform.Find("Border").AddComponent<LineRenderer>();

        LineRenderer lineRenderer = transform.Find("Border").GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4; 
        lineRenderer.loop = true;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, size), new Keyframe(1f, size), });

        lineRenderer.material = ContructionController.Instance.borderContructionMaterial;

        BoxCollider boxCollider = GetComponent<BoxCollider>(); // Lấy BoxCollider từ đối tượng

        // Tính toán các điểm bên dưới của hộp đựng
        Vector3 center = boxCollider.bounds.center;
        Vector3 extents = boxCollider.bounds.extents;
        Vector3 bottomFrontLeft = center - new Vector3(extents.x, extents.y, extents.z);
        Vector3 bottomFrontRight = center - new Vector3(-extents.x, extents.y, extents.z);
        Vector3 bottomBackLeft = center - new Vector3(extents.x, extents.y, -extents.z);
        Vector3 bottomBackRight = center - new Vector3(-extents.x, extents.y, -extents.z);

        // Sắp xếp các điểm để tạo thành hình chữ nhật
        Vector3[] rectanglePoints = new Vector3[4];
        rectanglePoints[0] = bottomFrontLeft;
        rectanglePoints[1] = bottomFrontRight;
        rectanglePoints[2] = bottomBackRight;
        rectanglePoints[3] = bottomBackLeft;
        rectanglePoints[0].y = this.transform.position.y;
        rectanglePoints[1].y = this.transform.position.y;
        rectanglePoints[2].y = this.transform.position.y;
        rectanglePoints[3].y = this.transform.position.y;

        // Đặt vị trí cho LineRenderer bằng các điểm bên dưới đã tính toán
        for (int i = 0; i < rectanglePoints.Length; i++)
        {
            lineRenderer.SetPosition(i, rectanglePoints[i] + new Vector3(0, 0.01f, 0)); // Dịch lên 0.01f từ mặt dưới cùng
        }



        /*for (int i = 0; i < PointBottomBoxCollider.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(PointBottomBoxCollider[i].x, PointBottomBoxCollider[i].y + 0.01f, PointBottomBoxCollider[i].z));
        }*/
    }

}
