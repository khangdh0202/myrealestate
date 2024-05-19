using UnityEngine;
using KhangLibrary;
public class ColliderWithBounds : MonoBehaviour
{
    [Tooltip("Chứa thông tin bound của snap area")] private Bounds thisBoundOfBuilding;
    private BoxCollider areaBuildingCollider;

    [Tooltip("Chứa 2 điểm va chạm cả object trên map")]private Vector3[] collisonPoint = new Vector3[2];

    [Tooltip("Chứa 2 điểm là đường thẳng song song và gần nhất với 2 điểm đã va chạm trên map")]private Vector3[] thisBuildingLineNearestAndParallelCollisonPoint = new Vector3[2];

    private GameObject buildingObject;
    private string directionCollison="";
    private float directionToBuilding = 0f;

    //bool havedDirectionCollison;

    public Bounds ThisBoundOfBuilding { get => thisBoundOfBuilding; set => thisBoundOfBuilding = value; }
    public string DirectionCollison { get => directionCollison; set => directionCollison = value; }
    public float DirectionToBuilding { get => directionToBuilding; set => directionToBuilding = value; }
    public Vector3[] CollisonPoint { get => collisonPoint; set => collisonPoint = value; }
    public BoxCollider AreaBuildingCollider { get => areaBuildingCollider; set => areaBuildingCollider = value; }
    public Vector3 BuildingOnMapGizmos { get => buildingOnMapGizmos; set => buildingOnMapGizmos = value; }
    public Vector3 PerpendicularPointGizmos { get => perpendicularPointGizmos; set => perpendicularPointGizmos = value; }

    public void Init(GameObject x)
    {

        /*if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }*/

        this.transform.position = x.GetComponent<BoxCollider>().bounds.center;
        transform.localScale = x.transform.localScale;
        transform.rotation = x.transform.rotation;

        /*gameObject.AddComponent<BoxCollider>();

        gameObject.GetComponent<BoxCollider>().size = x.GetComponent<BoxCollider>().size;*/

        /*// Lấy MeshFilter của đối tượng hiện tại
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

        // Lấy MeshFilter của đối tượng cha (parent) tại vị trí i
        MeshFilter parentMeshFilter = x.GetComponent<MeshFilter>();

        // Kiểm tra xem cả hai MeshFilter có tồn tại không
        if (currentMeshFilter != null && parentMeshFilter != null)
        {
            // Gán mesh của đối tượng cha cho đối tượng hiện tại
            currentMeshFilter.mesh = parentMeshFilter.sharedMesh;
        }

        // Lấy MeshRenderer của đối tượng hiện tại
        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();

        // Lấy MeshRenderer của đối tượng cha (parent) tại vị trí i
        MeshRenderer parentMeshRenderer = x.GetComponent<MeshRenderer>();

        // Kiểm tra xem cả hai MeshRenderer có tồn tại không
        if (currentMeshRenderer != null && parentMeshRenderer != null)
        {
            // Sao chép thông tin của Material từ MeshRenderer cha sang MeshRenderer hiện tại
            currentMeshRenderer.materials = parentMeshRenderer.sharedMaterials;
        }*/

        /*// Lấy BoxCollider của đối tượng hiện tại
        BoxCollider currentBoxCollider = GetComponent<BoxCollider>();

        // Lấy BoxCollider của đối tượng cha (parent)
        BoxCollider parentBoxCollider = x.GetComponent<BoxCollider>();

        // Kiểm tra xem cả hai BoxCollider có tồn tại không
        if (currentBoxCollider != null && parentBoxCollider != null)
        {
            // Sao chép thông tin BoxCollider từ đối tượng cha sang đối tượng hiện tại
            currentBoxCollider.center = parentBoxCollider.center;
            currentBoxCollider.size = parentBoxCollider.size;
        }
*/

        AreaBuildingCollider = ContructionController.Instance.ContructionBuild.currentBuidingInMouse.GetComponent<BoxCollider>();
        AreaBuildingCollider.isTrigger = true;

        this.name = Settings.snapAreaName;
         if (this.GetComponent<Renderer>().materials != null)
         {
            this.GetComponent<Renderer>().materials = new Material[0];
         }

        CreateDirectionCollison(x);
    }
    GameObject rightCollison, leftCollison, frontCollison, backCollison;
    public Vector3 ratioBoundAndCollider;
    public void CreateDirectionCollison(GameObject obj)
    {

        rightCollison = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftCollison = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frontCollison = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backCollison = GameObject.CreatePrimitive(PrimitiveType.Cube);

        rightCollison.GetComponent<BoxCollider>().isTrigger = true;
        leftCollison.GetComponent<BoxCollider>().isTrigger = true;
        frontCollison.GetComponent<BoxCollider>().isTrigger = true;
        backCollison.GetComponent<BoxCollider>().isTrigger = true;


        if (rightCollison.GetComponent<Rigidbody>() == null)
        {
            rightCollison.AddComponent<Rigidbody>();
            rightCollison.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (leftCollison.GetComponent<Rigidbody>() == null)
        {
            leftCollison.AddComponent<Rigidbody>();
            leftCollison.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (frontCollison.GetComponent<Rigidbody>() == null)
        {
            frontCollison.AddComponent<Rigidbody>();
            frontCollison.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (backCollison.GetComponent<Rigidbody>() == null)
        {
            backCollison.AddComponent<Rigidbody>();
            backCollison.GetComponent<Rigidbody>().isKinematic = true;
        }

        rightCollison.transform.parent = transform;
        leftCollison.transform.parent = transform;
        frontCollison.transform.parent = transform;
        backCollison.transform.parent = transform;

        // Lấy BoxCollider của đối tượng cha
        BoxCollider parentCollider = obj.GetComponent<BoxCollider>();
        // Lấy kích thước của BoxCollider của đối tượng cha
        Vector3 parentSize = parentCollider.bounds.size;

        // Tính toán tỉ lệ giữa bounds.size và size của đối tượng cha
        ratioBoundAndCollider = new Vector3(
            parentSize.x != 0 ? parentCollider.size.x / parentSize.x : 1f,
            parentSize.y != 0 ? parentCollider.size.y / parentSize.y : 1f,
            parentSize.z != 0 ? parentCollider.size.z / parentSize.z : 1f
        );

        UpdateDirectionCollison(obj, ratioBoundAndCollider);


        

        /*Vector3 value = obj.GetComponent<BoxCollider>().size;
        Debug.Log("value " + value);

        rightCollison.transform.localScale = new Vector3(0.2f, 1, 1);
        leftCollison.transform.localScale = new Vector3(0.2f, 1, 1);
        frontCollison.transform.localScale = new Vector3(1, 1, 0.2f);
        backCollison.transform.localScale = new Vector3(1, 1, 0.2f);

        rightCollison.GetComponent<BoxCollider>().size = value;
        leftCollison.GetComponent<BoxCollider>().size = value;
        frontCollison.GetComponent<BoxCollider>().size = value;
        backCollison.GetComponent<BoxCollider>().size = value;

        // Lấy BoxCollider của đối tượng hiện tại
        BoxCollider currentBoxCollider = obj.GetComponent<BoxCollider>();

        rightCollison.transform.position = currentBoxCollider.bounds.center;
        leftCollison.transform.position = currentBoxCollider.bounds.center;
        frontCollison.transform.position = currentBoxCollider.bounds.center;
        backCollison.transform.position = currentBoxCollider.bounds.center;

        value = obj.GetComponent<BoxCollider>().bounds.size;

        rightCollison.transform.Translate(Vector3.right * (value.x / 2f), Space.Self);
        leftCollison.transform.Translate(Vector3.left * (value.x / 2f), Space.Self);
        frontCollison.transform.Translate(Vector3.back * (value.z / 2f), Space.Self);
        backCollison.transform.Translate(Vector3.forward * (value.z / 2f), Space.Self);

        rightCollison.transform.Translate(Vector3.right * (value.x / 5f)/2f, Space.Self);
        leftCollison.transform.Translate(Vector3.left * (value.x / 5f)/2f, Space.Self);
        frontCollison.transform.Translate(Vector3.back * (value.z / 5f)/2f, Space.Self);
        backCollison.transform.Translate(Vector3.forward * (value.z / 5f)/2f, Space.Self);*/

        /*// di chuyển các collision va  chạm từ tâm ra cạnh
        rightCollison.transform.position = rightCollison.transform.position + Vector3.right * (value.x / 2);
        leftCollison.transform.position = leftCollison.transform.position + Vector3.left * (value.x / 2);
        frontCollison.transform.position = frontCollison.transform.position + Vector3.back * (value.z / 2);
        backCollison.transform.position = backCollison.transform.position + Vector3.forward * (value.z / 2);

        // Điều chỉnh vị trí của right và left trên trục x
        rightCollison.transform.position += Vector3.right * (rightCollison.transform.localScale.x * (value.x / 2)) ;
        leftCollison.transform.position += Vector3.left * (leftCollison.transform.localScale.x * (value.x / 2));

        // Điều chỉnh vị trí của front và back trên trục z
        frontCollison.transform.position += Vector3.back * (frontCollison.transform.localScale.z * (value.z / 2));
        backCollison.transform.position += Vector3.forward * (backCollison.transform.localScale.z * (value.z / 2));*/

        rightCollison.name = "right";
        leftCollison.name = "left";
        frontCollison.name = "front";
        backCollison.name = "back";

        if (rightCollison.GetComponent<Renderer>().materials != null)
        {
            rightCollison.GetComponent<Renderer>().materials = new Material[0];
        }
        if (leftCollison.GetComponent<Renderer>().materials != null)
        {
            leftCollison.GetComponent<Renderer>().materials = new Material[0];
        }
        if (frontCollison.GetComponent<Renderer>().materials != null)
        {
            frontCollison.GetComponent<Renderer>().materials = new Material[0];
        }
        if (backCollison.GetComponent<Renderer>().materials != null)
        {
            backCollison.GetComponent<Renderer>().materials = new Material[0];
        }

        rightCollison.AddComponent<DirectionCollison>();
        leftCollison.AddComponent<DirectionCollison>();
        frontCollison.AddComponent<DirectionCollison>();
        backCollison.AddComponent<DirectionCollison>();
    }
    public void UpdateDirectionCollison(GameObject obj , Vector3 ratioBoundAndCollider)
    {
        Vector3 value = obj.GetComponent<BoxCollider>().size;
        //Debug.Log("BoxSize " + obj.GetComponent<BoxCollider>().bounds.size);

        rightCollison.transform.localScale = new Vector3(0.2f, 1, 1);
        leftCollison.transform.localScale = new Vector3(0.2f, 1, 1);
        frontCollison.transform.localScale = new Vector3(1, 1, 0.2f);
        backCollison.transform.localScale = new Vector3(1, 1, 0.2f);

        //Debug.Log("BoxSize1 " + ratioBoundAndCollider);

        // Lấy kích thước của BoxCollider của đối tượng con
        Vector3 childSize = obj.GetComponent<BoxCollider>().bounds.size;

        // Cập nhật kích thước của các collision object
        rightCollison.GetComponent<BoxCollider>().size = new Vector3(childSize.x * ratioBoundAndCollider.x, childSize.y * ratioBoundAndCollider.y, childSize.z * ratioBoundAndCollider.z);
        leftCollison.GetComponent<BoxCollider>().size = new Vector3(childSize.x * ratioBoundAndCollider.x, childSize.y * ratioBoundAndCollider.y, childSize.z * ratioBoundAndCollider.z);
        frontCollison.GetComponent<BoxCollider>().size = new Vector3(childSize.x * ratioBoundAndCollider.x, childSize.y * ratioBoundAndCollider.y, childSize.z * ratioBoundAndCollider.z);
        backCollison.GetComponent<BoxCollider>().size = new Vector3(childSize.x * ratioBoundAndCollider.x, childSize.y * ratioBoundAndCollider.y, childSize.z * ratioBoundAndCollider.z);
        /*rightCollison.GetComponent<BoxCollider>().size = value + value * sizeRatio.x;
        leftCollison.GetComponent<BoxCollider>().size = value + value * sizeRatio.x;
        frontCollison.GetComponent<BoxCollider>().size = value + value * sizeRatio.z; 
        backCollison.GetComponent<BoxCollider>().size = value + value * sizeRatio.z;*/

        /*rightCollison.GetComponent<BoxCollider>().size = value;
        leftCollison.GetComponent<BoxCollider>().size = value;
        frontCollison.GetComponent<BoxCollider>().size = value;
        backCollison.GetComponent<BoxCollider>().size = value;*/

        // Lấy BoxCollider của đối tượng hiện tại
        BoxCollider currentBoxCollider = obj.GetComponent<BoxCollider>();

        rightCollison.transform.position = currentBoxCollider.bounds.center;
        leftCollison.transform.position = currentBoxCollider.bounds.center;
        frontCollison.transform.position = currentBoxCollider.bounds.center;
        backCollison.transform.position = currentBoxCollider.bounds.center;

        value = obj.GetComponent<BoxCollider>().bounds.size;

        rightCollison.transform.Translate(Vector3.right * (value.x / 2f), Space.Self);
        leftCollison.transform.Translate(Vector3.left * (value.x / 2f), Space.Self);
        frontCollison.transform.Translate(Vector3.back * (value.z / 2f), Space.Self);
        backCollison.transform.Translate(Vector3.forward * (value.z / 2f), Space.Self);

        rightCollison.transform.Translate(Vector3.right * (value.x / 5f) / 2f, Space.Self);
        leftCollison.transform.Translate(Vector3.left * (value.x / 5f) / 2f, Space.Self);
        frontCollison.transform.Translate(Vector3.back * (value.z / 5f) / 2f, Space.Self);
        backCollison.transform.Translate(Vector3.forward * (value.z / 5f) / 2f, Space.Self);
    }

    private void Update()
    {
        if (AreaBuildingCollider.bounds != null)
        {
            ThisBoundOfBuilding = AreaBuildingCollider.bounds;
        }
    }

    Vector3 CalculateSizeRatio(GameObject obj)
    {
        // Lấy kích thước của BoxCollider
        Vector3 colliderSize = obj.GetComponent<BoxCollider>().size;

        // Lấy kích thước của bounds
        Bounds bounds = obj.GetComponent<BoxCollider>().bounds;
        Vector3 boundsSize = bounds.size;

        // Tính toán tỉ lệ giữa kích thước của BoxCollider và kích thước của bounds
        float xRatio = boundsSize.x / colliderSize.x;
        float yRatio = boundsSize.y / colliderSize.y;
        float zRatio = boundsSize.z / colliderSize.z;

        return new Vector3(xRatio, yRatio, zRatio);
    }

    Vector3 buildingOnMapGizmos, perpendicularPointGizmos;

    Vector3 right1, right2, left1, left2, front1, front2, back1, back2;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Vẽ bounding box bằng hàm OnDrawGizmos
        //Gizmos.DrawWireCube(ThisBoundOfBuilding.center, ThisBoundOfBuilding.size);

        Gizmos.DrawLine(right1, right2);
        Gizmos.DrawLine(left1, left2);
        Gizmos.DrawLine(front1, front2);
        Gizmos.DrawLine(back1, back2);

        Gizmos.color = Color.green;

        // Vẽ các điểm dưới trên Gizmos để kiểm tra
        //Gizmos.DrawSphere(perpendicularPointGizmos, 0.2f);
        Gizmos.DrawSphere(CollisonPoint[0], 0.2f);
        Gizmos.DrawSphere(CollisonPoint[1], 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(thisBuildingLineNearestAndParallelCollisonPoint[0], 0.2f);
        Gizmos.DrawSphere(thisBuildingLineNearestAndParallelCollisonPoint[1], 0.2f);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(BuildingOnMapGizmos, 0.2f);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(CollisonPoint[0], CollisonPoint[1]);

        Gizmos.color = Color.yellow;
        //if (LineIntersection3D.ArePerpendicular(buildingOnMapGizmos, LineIntersection3D.GetPerpendicularPoint(buildingOnMapGizmos, thisBuildingLineNearestAndParallelCollisonPoint[0], thisBuildingLineNearestAndParallelCollisonPoint[1]), thisBuildingLineNearestAndParallelCollisonPoint[0], thisBuildingLineNearestAndParallelCollisonPoint[1]))
        //{
            Gizmos.DrawLine(thisBuildingLineNearestAndParallelCollisonPoint[0], thisBuildingLineNearestAndParallelCollisonPoint[1]);
        //}
        //Gizmos.DrawLine(buildingOnMapGizmos, perpendicularPointGizmos);
    }
}
