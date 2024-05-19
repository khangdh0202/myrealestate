using UnityEngine;
using KhangLibrary;
using Unity.VisualScripting;
public class DirectionCollison : MonoBehaviour
{
    private bool localSnapToPoint = false;

    const float threshold = 0.1f;

    GameObject[] nearestPointObject = new GameObject[2];

    Vector3 hitVectorInMap;

    private void Update()
    {
        if (Settings.EndConstructionExit || !Settings.isSnaping)
        {
            if (transform.parent.GetComponent<ColliderWithBounds>() != null)
            {
                transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison = "";
                transform.parent.GetComponent<ColliderWithBounds>().CollisonPoint = new Vector3[2];
                Settings.isSnaping = false;
                PlacementSystem.Instance.directionSnap = "";

                localSnapToPoint = false;
                Settings._snapToPointBottomOfBuilding = false;
                Settings._distanceFromPointToPointBottomOfBuilding = 0;
                Settings._directionSnaptoPointBottomOfBuilding = 0;
                Settings.buildingSnapToRoad = false;
                Settings._snapToConner = false;
                nearestPointObject[0] = null;
                nearestPointObject[1] = null;
            }  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        #region

        if (other.gameObject.layer != LayerMask.NameToLayer("Building"))
            return;

        if (other.gameObject == ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
        {
            return;
        }
        #endregion

        nearestPointObject[0] = null;
        nearestPointObject[1] = null;
        if (transform.parent.GetComponent<ColliderWithBounds>() != null)
        {
            BoxCollider thisCollider = this.GetComponent<BoxCollider>();
            BoxCollider otherCollider = other.gameObject.GetComponent<BoxCollider>();

            if (thisCollider != null && otherCollider != null)
            {
                Vector3 myTransform = new Vector3(transform.position.x, transform.parent.parent.transform.position.y, transform.position.z);

                Vector3 direction = myTransform - ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.position;

                direction.x = Mathf.Abs(direction.x) < threshold ? 0f : direction.x;
                direction.y = Mathf.Abs(direction.y) < threshold ? 0f : direction.y;
                direction.z = Mathf.Abs(direction.z) < threshold ? 0f : direction.z;
                point5 = myTransform;
                point6 = ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.position;

                GameObject[] pointOtherBuilding = other.gameObject.GetComponent<BoundingBoxBuilding>().PointOfBuildingObject;

                float distance = float.MaxValue;

                for (int i = 0; i < pointOtherBuilding.Length; i++)
                {
                    Vector3 directionTwoPoint = pointOtherBuilding[i].transform.position - pointOtherBuilding[i == pointOtherBuilding.Length - 1 ? 0 : i + 1].transform.position;

                    if (directionTwoPoint.normalized == direction.normalized || -directionTwoPoint.normalized == direction.normalized)
                    {
                        Debug.Log("Song song");
                        continue;
                    }
                    Vector3 diemVuongGoc = LineIntersection3D.GetPerpendicularPoint(pointOtherBuilding[i].transform.position, pointOtherBuilding[i == pointOtherBuilding.Length - 1 ? 0 : i + 1].transform.position, myTransform);

                    float value = Vector3.Distance(myTransform, diemVuongGoc);
                    if (distance > value)
                    {
                        distance = value;
                        nearestPointObject[0] = pointOtherBuilding[i];
                        nearestPointObject[1] = pointOtherBuilding[i == pointOtherBuilding.Length - 1 ? 0 : i + 1];
                        //nearestPointObject = new GameObject[] { pointOtherBuilding[i], pointOtherBuilding[i == pointOtherBuilding.Length - 1 ? 0 : i + 1] };
                        point3 = nearestPointObject[0].transform.position;
                        point4 = nearestPointObject[1].transform.position;
                    }
                }
            }
            else
            {
                Debug.LogError("Một trong hai đối tượng không có hình hộp (BoxCollider).");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        #region Dieu kien

        if (other.gameObject == ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
        {
            Debug.Log("Exit1");
            return;
        }
        
        if (Settings.insideConstructionOther)
        {
            return;
        }

        if (other.CompareTag("Building") && SettingManager.Instance.snapToConner)
        {
            foreach (GameObject point in ContructionController.Instance.ContructionBuild.currentBuidingInMouse.GetComponent<BoundingBoxBuilding>().PointOfBuildingObject)
            {
                if (this.transform.parent.GetComponent<ColliderWithBounds>().ThisBoundOfBuilding.Contains(new Vector3(Settings.currentPositionRay.x, Settings.currentPositionRay.y, Settings.currentPositionRay.z)))
                {
                    Bounds bounds = point.GetComponent<BoxCollider>().bounds;
                    if (nearestPointObject[0])
                    {
                        if (bounds.Contains(nearestPointObject[0].transform.position))
                        {
                            // diem tren đường thẳng point1 - point2 
                            Vector3 vectorOnLine = LineIntersection3D.GetPerpendicularPoint(nearestPointObject[0].transform.position, nearestPointObject[1].transform.position, point.transform.position);

                            Vector3 direction = nearestPointObject[0].transform.position - vectorOnLine;
                            direction.y = 0;
                            float distance = Vector3.Distance(nearestPointObject[0].transform.position, vectorOnLine);

                            Vector3 parPositon = transform.parent.parent.position;
                            transform.parent.parent.position = parPositon + direction.normalized * distance;

                            Settings.currentPositionRay = transform.parent.parent.position;

                            //Settings._snapToConner = true;
                            return;

                        }
                    }
                    if (nearestPointObject[1])
                    {
                        if (bounds.Contains(nearestPointObject[1].transform.position))
                        {
                            // diem tren đường thẳng point1 - point2 .currentPos
                            Vector3 vectorOnLine = LineIntersection3D.GetPerpendicularPoint(nearestPointObject[0].transform.position, nearestPointObject[1].transform.position, point.transform.position);

                            Vector3 direction = nearestPointObject[1].transform.position - vectorOnLine;
                            direction.y = 0;

                            float distance = Vector3.Distance(nearestPointObject[1].transform.position, vectorOnLine);

                            Vector3 parPositon = transform.parent.parent.position;
                            //Debug.Log($"distance {distance} - direction.normalized {direction.normalized} - parPositon {parPositon}");
                            transform.parent.parent.position = parPositon + direction.normalized * distance;

                            //Settings._snapToConner = true;
                            return;
                        }
                    }
                }
            }
        }

        /*if (Settings._snapToConner)
        {
            Debug.Log("Exit3");
            return;
        }*/

        if (Settings.isSnaping && !localSnapToPoint)
        {
            Debug.Log("Exit2");
            return;
        }

        /*//if (Settings._snapToPointBottomOfBuilding && !localSnapToPoint)
        if (Settings.isSnaping && !localSnapToPoint)
        {
            Debug.Log("Exit4");
            return;
        }*/

        /*if (Settings.isSnaping && !localSnapToPoint && transform != Settings.directionSnapingBuilding)
        {
            Debug.Log("Exit5");
            return;
        }*/

        #endregion

        if (other.CompareTag("Road"))
        {

        }
        if (other.CompareTag("Building"))
        {
            if (nearestPointObject[0] && nearestPointObject[1])
            {
                float value;

                if (PlacementSystem.Instance.directionSnap == "" && !Settings.isSnaping && !localSnapToPoint
                    )
                {
                    Vector3 myTransform = new Vector3(transform.position.x, transform.parent.parent.transform.position.y, transform.position.z);

                    value = Vector3.Distance(myTransform, LineIntersection3D.GetPerpendicularPoint(nearestPointObject[0].transform.position, nearestPointObject[1].transform.position, myTransform));

                    Vector3 direction = myTransform - ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.position;

                    direction.x = Mathf.Abs(direction.x) < threshold ? 0f : direction.x;
                    direction.y = Mathf.Abs(direction.y) < threshold ? 0f : direction.y;
                    direction.z = Mathf.Abs(direction.z) < threshold ? 0f : direction.z;

                    transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison = gameObject.name;
                    if (transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison == "right")
                    {
                        PlacementSystem.Instance.directionSnap = "right";
                        Settings.currentPositionRay = PlacementSystem.Instance.UpdatePositionMouseIndicator(direction.normalized, PlacementSystem.Instance.mouseIndicator, value + transform.gameObject.GetComponent<BoxCollider>().bounds.size.x / 2f, PlacementSystem.Instance.directionSnap);

                    }
                    else if (transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison == "left")
                    {
                        PlacementSystem.Instance.directionSnap = "left";
                        Settings.currentPositionRay = PlacementSystem.Instance.UpdatePositionMouseIndicator(direction.normalized, PlacementSystem.Instance.mouseIndicator, value + transform.gameObject.GetComponent<BoxCollider>().bounds.size.x / 2f, PlacementSystem.Instance.directionSnap);

                    }
                    else if (transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison == "front")
                    {
                        PlacementSystem.Instance.directionSnap = "front";
                        Settings.currentPositionRay = PlacementSystem.Instance.UpdatePositionMouseIndicator(direction.normalized, PlacementSystem.Instance.mouseIndicator, value + transform.gameObject.GetComponent<BoxCollider>().bounds.size.z / 2f, PlacementSystem.Instance.directionSnap);
                    }
                    else if (transform.parent.GetComponent<ColliderWithBounds>().DirectionCollison == "back")
                    {
                        PlacementSystem.Instance.directionSnap = "back";
                        Settings.currentPositionRay = PlacementSystem.Instance.UpdatePositionMouseIndicator(direction.normalized, PlacementSystem.Instance.mouseIndicator, value + transform.gameObject.GetComponent<BoxCollider>().bounds.size.z / 2f, PlacementSystem.Instance.directionSnap);
                    }
                }

                if (Settings.isSnaping)
                {
                    Vector3 myTransform = new Vector3(transform.position.x, transform.parent.parent.transform.position.y, transform.position.z);

                    // Settings.currentPositionRay = điểm laze controller chỉ vào
                    hitVectorInMap = LineIntersection3D.GetPerpendicularPoint(nearestPointObject[0].transform.position, nearestPointObject[1].transform.position, Settings.currentPositionRay);

                    point1 = nearestPointObject[0].transform.position;
                    point2 = nearestPointObject[1].transform.position;

                    Vector3 pointDirectionCollisonInEdge = LineIntersection3D.GetPerpendicularPoint(nearestPointObject[0].transform.position, nearestPointObject[1].transform.position, myTransform);

                    Vector3 direction = hitVectorInMap - pointDirectionCollisonInEdge;

                    float vectorDoLech = Vector3.Distance(hitVectorInMap,pointDirectionCollisonInEdge);

                    Vector3 parPositon = transform.parent.parent.position;
                    Vector3 newVector = parPositon + direction.normalized* vectorDoLech;
                    //Debug.Log("CCCC direction" + direction);
                    newVector = new Vector3(newVector.x, parPositon.y, newVector.z);

                    transform.parent.parent.position = newVector;

                    Settings.currentPositionRay = transform.parent.parent.position;
                }
                
                if (!localSnapToPoint)
                {
                    localSnapToPoint = true;
                    Settings._snapToPointBottomOfBuilding = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Settings._snapToConner)
            Settings._snapToConner = false;
    }

    public bool AreColliding(BoxCollider boxCollider1, BoxCollider boxCollider2)
    {
        // Lấy các biên giới của hai BoxCollider
        Bounds bounds1 = boxCollider1.bounds;
        Bounds bounds2 = boxCollider2.bounds;

        // Kiểm tra xem hai biên giới có giao nhau không
        bool isColliding = bounds1.Intersects(bounds2);

        return isColliding;
    }

    /// <summary>
    /// point1, point2 là 2 điểm của công trình trên map của cạnh va chạm
    /// </summary>

    Vector3 point1, point2;
    Vector3 point3, point4;
    Vector3 point5, point6;

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawSphere(point1, 0.2f);
        Gizmos.DrawSphere(point2, 0.2f);

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawSphere(point3, 0.11f);
        Gizmos.DrawSphere(point4, 0.11f);
        Gizmos.DrawLine(point5, point6);


    }
    public GameObject[] FindNearestObjects(GameObject[] objectsArray, Vector3 inMap)
    {
        // Khởi tạo biến lưu trữ 2 đối tượng gần nhất
        GameObject nearestObject1 = null;
        GameObject nearestObject2 = null;

        // Khởi tạo biến lưu trữ khoảng cách tối thiểu
        float minDistance1 = Mathf.Infinity;
        float minDistance2 = Mathf.Infinity;

        // Duyệt qua mảng các đối tượng
        foreach (GameObject obj in objectsArray)
        {
            // Tính khoảng cách giữa đối tượng và điểm inMap
            float distance = Vector3.Distance(obj.transform.position, inMap);

            // So sánh với khoảng cách tối thiểu
            if (distance < minDistance1)
            {
                // Nếu khoảng cách nhỏ hơn minDistance1, cập nhật minDistance1 và nearestObject1
                minDistance2 = minDistance1;
                nearestObject2 = nearestObject1;
                minDistance1 = distance;
                nearestObject1 = obj;
            }
            else if (distance < minDistance2)
            {
                // Nếu khoảng cách nằm giữa minDistance1 và minDistance2, cập nhật minDistance2 và nearestObject2
                minDistance2 = distance;
                nearestObject2 = obj;
            }
        }

        // Tạo một mảng chứa 2 đối tượng gần nhất và trả về
        GameObject[] nearestObjects = { nearestObject1, nearestObject2 };
        return nearestObjects;
    }

    /// <summary>
    /// collider1 must have a rigidbody
    /// </summary>
    /// <param name="collider1"></param>
    /// <param name="collider2"></param>
    /// <param name="collisonPoint"></param>
    /// <returns></returns>
    Vector3[] GetContactPoints(BoxCollider collider1, BoxCollider collider2)
    {

        // Lấy các điểm tiếp xúc giữa hai BoxCollider
        bool success = Physics.ComputePenetration(collider1, collider1.transform.position, collider1.transform.rotation,
                                                  collider2, collider2.transform.position, collider2.transform.rotation,
                                                  out Vector3 direction, out float distance);
        Vector3[] collisonPoint = new Vector3[2];

        // Nếu có va chạm, lấy các điểm tiếp xúc
        if (success)
        {
            collisonPoint[0] = collider1.ClosestPoint(collider2.transform.position);
            collisonPoint[1] = collider2.ClosestPoint(collider1.transform.position);
            return collisonPoint;
        }
        else
        {
            return null;
        }
    }
}
