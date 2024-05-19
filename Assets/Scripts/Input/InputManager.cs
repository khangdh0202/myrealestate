using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : SingletonMonobehaviours<InputManager>
{
    [SerializeField]
    private GameObject leftController; // Tham chiếu đến VR controller trái
    [SerializeField]
    private GameObject rightController; // Tham chiếu đến VR controller phải

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    public LayerMask placementLayerMask;

    private void Start()
    {
        leftController = Player.Instance.transform.Find("Camera Offset").Find("LeftHand Father").Find("LeftHand UI Controller").gameObject;
        rightController = Player.Instance.transform.Find("Camera Offset").Find("RightHand Father").Find("RightHand UI Controller").gameObject;
    }

    /// <summary>
    /// Kiểm tra xem con trỏ có bận không   
    /// </summary>
    public static bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 ChangeController(int x)
    {
        if (leftController && x == 0)
        {
            // Lấy vị trí của VR controller trái
            Vector3 controllerPosition = leftController.transform.position;

            // Tạo một ray từ vị trí của VR controller trái
            Ray ray = new Ray(controllerPosition, leftController.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
            {
                return hit.point;
            }
        }
        else if (rightController && x == 1)
        {
            // Lấy vị trí của VR controller trái
            Vector3 controllerPosition = rightController.transform.position;

            // Tạo một ray từ vị trí của VR controller trái
            Ray ray = new Ray(controllerPosition, rightController.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
            {
                return hit.point;
            }
        }
        return PlacementSystem.Instance.rayCheckCollision.transform.position;
    }

    public Vector3 GetSelectedMapPosition(int x, string directionSnap, float value)
    {
        if (PlacementSystem.Instance.mouseIndicator)
        {
            if (PlacementSystem.Instance.mouseIndicator.transform.position != null)
            {
                lastPosition = PlacementSystem.Instance.mouseIndicator.transform.position;
            }
        }

        Settings.currentPositionRay = lastPosition;

        if (!Settings._canSnapFromPointToPointBottomOfBuilding && value == 0f)
            return lastPosition;


        if (PlacementSystem.Instance.haveObjectInMouseIndicator)
        {
            //lastPosition = Settings.currentPositionRay;
            if (leftController && x == 0)
            {
                // Lấy vị trí của VR controller trái
                Vector3 controllerPosition = leftController.transform.position;

                // Tạo một ray từ vị trí của VR controller trái
                Ray ray = new Ray(controllerPosition, leftController.transform.forward);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
                {
                    PlacementSystem.Instance.rayCheckCollision.transform.position = hit.point;

                    if (directionSnap == "")
                    {
                        lastPosition = hit.point;
                        Settings.isSnaping = false;
                    }
                    else if (directionSnap != "")
                    {
                        if (Settings.isSnaping)
                        {
                            if (Settings._snapToPointBottomOfBuilding || Settings.buildingSnapToRoad)
                            {
                                Settings.currentPositionRay = hit.point;
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            Settings.isSnaping = true;
                        }
                        if (Settings.RoadSnapToRoad && !Settings.EndRoadExit)
                        {
                            lastPosition = Settings.positionRoadSnapToRoad;
                        }
                    }
                }

            }
            else if (rightController && x == 1)
            {
                // Lấy vị trí của VR controller trái
                Vector3 controllerPosition = rightController.transform.position;

                // Tạo một ray từ vị trí của VR controller trái
                Ray ray = new Ray(controllerPosition, rightController.transform.forward);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
                {
                    PlacementSystem.Instance.rayCheckCollision.transform.position = hit.point;

                    if (directionSnap == "")
                    {
                        lastPosition = hit.point;
                        Settings.isSnaping = false;
                    }
                    else if (directionSnap != "")
                    {
                        if (Settings.isSnaping)
                        {
                            if (Settings._snapToPointBottomOfBuilding || Settings.buildingSnapToRoad)
                            {
                                Settings.currentPositionRay = hit.point;
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            Settings.isSnaping = true;
                        }
                        if (Settings.RoadSnapToRoad && !Settings.EndRoadExit)
                        {
                            lastPosition = Settings.positionRoadSnapToRoad;
                        }
                    }
                }
            }
        }
        
        return lastPosition;
    }

    Vector3 test1;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(test1, 0.1f);
    }
}
