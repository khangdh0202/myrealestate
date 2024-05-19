using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlacementSystem : SingletonMonobehaviours<PlacementSystem>
{
    [SerializeField] private string typeOfMouseIndicator;
    public GameObject rayCheckCollision;
    public GameObject mouseIndicator;// cellIndicator;

    public List<string> keyDeleteObject;

    public bool haveObjectInMouseIndicator;
    public string directionSnap;
    public string directionRoadSnap;
    public Vector3 positionValue;

    [SerializeField]
    private Camera cameraScene;

    [SerializeField]
    private Grid grid;

    public AudioClip buildingSound;
    public AudioClip roadSound;
    private AudioSource audioSource;

    // Thời gian được đặt 1 building
    private float timeBuildCounter, timeBuild;

    // Thời gian được xoay building
    public float timeRotationCounter, timeRotation;


    public string TypeOfMouseIndicator { get => typeOfMouseIndicator; set => typeOfMouseIndicator = value; }

    private void Start()
    {
        haveObjectInMouseIndicator = false;
        mouseIndicator = null;
        directionSnap = "";
        keyDeleteObject = new List<string>();

        timeBuild = 0.7f;
        timeBuildCounter = 0;
        timeRotation = 0.1f;
        timeRotationCounter = 0;

        audioSource = GetComponent<AudioSource>();
        AssignBuildingEffect(buildingSound);
    }
    private void Update()
    {
        TimeBuildCountdown(ref timeBuildCounter);
        TimeBuildCountdown(ref timeRotationCounter);

        audioSource.volume = Settings.volumeSFX * Settings.volumeMaster;

        Vector3 mousePosition;

        if (Settings.triggerButtonLeftController == true)
        {
            Settings.triggerButtonLeftController = true;
            Settings.triggerButtonRightController = false;
            mousePosition = InputManager.Instance.GetSelectedMapPosition(0, directionSnap, 0);
            MouseIndicatorToRayOfController(mousePosition);
        }
        else if (Settings.triggerButtonRightController == true)
        {
            Settings.triggerButtonRightController = true;
            Settings.triggerButtonLeftController = false;
            mousePosition = InputManager.Instance.GetSelectedMapPosition(1, directionSnap, 0);// nếu nút trigger right controller ấn => truyền 1
            MouseIndicatorToRayOfController(mousePosition);
        }
    }

    // Event
    public Action placeRoad, placeContruction;
    public void PutBuildingInMap()
    {
        if (mouseIndicator == null)
            return;
        if (!InputManager.IsPointerOverUI())
        {
            if (Settings.canBuild && timeBuildCounter <= 0)
            {
                if (mouseIndicator.CompareTag("Building") && !Settings.onGrid)
                {
                    return;
                }
                if (mouseIndicator == null)
                    return;

                TimeBuildCooldown(ref timeBuildCounter, timeBuild);
                
                // Item type model
                if (TypeOfMouseIndicator == "Build" ||
                    TypeOfMouseIndicator == "Scenary" ||
                    TypeOfMouseIndicator == "Prop")
                {
                    audioSource.clip = buildingSound;
                    placeContruction?.Invoke();
                }

                // Item type road
                if(TypeOfMouseIndicator == "Road")
                {
                    audioSource.clip = roadSound;
                    placeRoad?.Invoke();
                }

                Settings.evenNumberYScale = false;

                if (Settings.inScale)
                {
                    Settings.inScale = !Settings.inScale;
                }
            }
        }
    }
    // chuyển tay cầm trái
    public void ItemInRightHand()
    {
        Debug.Log("1");
        Vector3 mousePosition;
        Settings.triggerButtonRightController = true;
        Settings.triggerButtonLeftController = false;
        mousePosition = InputManager.Instance.ChangeController(1);// nếu nút trigger right controller ấn => truyền 1
        MouseIndicatorToRayOfController(mousePosition);
    }
    // Chuyển tay cầm phải
    public void ItemInLeftHand()
    {
        Debug.Log("2");
        Vector3 mousePosition;
        Settings.triggerButtonLeftController = true;
        Settings.triggerButtonRightController = false;
        mousePosition = InputManager.Instance.ChangeController(0); // nếu nút trigger left controller ấn => truyền 0
        MouseIndicatorToRayOfController(mousePosition);
    }


    public Action<GameObject> deleteContruction, deleteRoad;
    // Event xóa item trên map
    public void DeleteItemInMap(GameObject obj)
    {
        if (obj.CompareTag(Settings.roadTag))
        {
            deleteRoad?.Invoke(obj);
        }
        else if (obj.CompareTag(Settings.contructionTag))
        {
            deleteContruction?.Invoke(obj);
        }
    }
    /// <summary>
    /// Delete Item in mouseIndicator
    /// </summary>
    public void CancelItem()
    {
        if (PlacementSystem.Instance.mouseIndicator != null)
        {
            for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
            {
                if (PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).name != Settings.snapAreaName)
                {
                    PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject.transform.position = new Vector3();
                    PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject.transform.rotation = Quaternion.Euler(new Vector3());
                    PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject.SetActive(false);
                    PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).transform.parent = null;
                    PlacementSystem.Instance.haveObjectInMouseIndicator = false;
                    PlacementSystem.Instance.directionSnap = "";
                }
            }
            CancelSnap();
            Destroy(PlacementSystem.Instance.mouseIndicator);
        }
    }

    /// <summary>
    /// 1 = right, 2 = left, 3 = forward, 4 = back
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    /// <param name="x"></param>
    public Vector3 UpdatePositionMouseIndicator(Vector3 distance, GameObject obj, float value , string x)
    {
        Vector3 vector = new Vector3();
        obj.transform.Translate(distance * value, Space.World);
        vector = obj.transform.position;

        return vector;
    }

    public void UpdateScaleMouseIndicator(GameObject obj,float x,float y,float z)
    {
        obj.transform.localScale = new Vector3(x, y, z);
        obj.transform.localScale = new Vector3(x, y, z);
    }

    /// <summary>
    /// hủy snap di chuyển tự do
    /// </summary>
    public void CancelSnap()
    {
        Settings.isSnaping = false;
        directionSnap = "";
        directionRoadSnap = "";
        Settings._snapToPointBottomOfBuilding = false;
        Settings._directionSnaptoPointBottomOfBuilding = 0;
        Settings._distanceFromPointToPointBottomOfBuilding = 0f;
        Settings._canSnapFromPointToPointBottomOfBuilding = true;
        Settings.roadSnaptoRoad = false;
    }

    /// <summary>
    /// Cập nhật vị trí của mouse Indicator vào vị trí của ray
    /// </summary>
    /// <param name="mousePosition"></param>
    public void MouseIndicatorToRayOfController(Vector3 mousePosition)
    {
        if (mouseIndicator)
        {
            mouseIndicator.transform.position = mousePosition;
        }
    }

    /// <summary>
    /// Đếm ngược thời gian được xây dựng
    /// </summary>
    public void TimeBuildCountdown(ref float timeBuildCounter)
    {
        if (timeBuildCounter > 0)
        {
            timeBuildCounter -= Time.deltaTime;
        }
        else
        {
            timeBuildCounter = 0;
        }
    }

    /// <summary>
    /// Thời gian chờ xây dựng
    /// </summary>
    public void TimeBuildCooldown(ref float timeBuildCounter, float timeBuild)
    {
        timeBuildCounter = timeBuild;
    }
    public void AssignBuildingEffect(AudioClip value)
    {
        audioSource = GetComponent<AudioSource>();

        buildingSound = value;
        audioSource.clip = buildingSound;
        audioSource.volume = Settings.volumeSFX * Settings.volumeMaster;
        audioSource.playOnAwake = false;
    }
    public void Sound()
    {
        audioSource.Play();
    }
}
