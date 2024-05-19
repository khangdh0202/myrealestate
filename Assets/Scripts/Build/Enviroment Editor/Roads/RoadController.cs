using FluffyUnderware.Curvy;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoadController : SingletonMonobehaviours<RoadController>, IDataPersistantce
{
    private GameObject currentObject;

    [SerializeField]
    private GameObject _roadPrefab;
    [SerializeField]
    private GameObject markerPrefab; // Prefab của cột đánh dấu
    [SerializeField]
    private GameObject textPrefabs; // Prefab của cột đánh dấu

    [SerializeField]
    private Transform point;

    [SerializeField, Tooltip("Điểm đầu điểm cuối tạo 1 con đường")] 
    private List<Transform> _roadTransform;

    [SerializeField, Tooltip("2 đường thẳng có 3 điểm, đây là điểm đầu tiên")] private GameObject firstPointOfTwoRoad = null;
    [SerializeField, Tooltip("Điểm đầu đường tạm")] private GameObject startRoadTemporary;
    [SerializeField, Tooltip("Điểm cuối đường tạm")] private GameObject endRoadTemporary;

    [SerializeField, Tooltip("Tại một điểm của road, sẽ có 2 điểm điều khiển độ cong của con đường hướng tới " +
        "điểm đầu đường thứ nhất và điểm cuối đường thứ hai, đây là điểm thứ 1")]
    GameObject objectOneCurvePointOfRoad = null;
    [SerializeField, Tooltip("Tại một điểm của road, sẽ có 2 điểm điều khiển độ cong của con đường hướng tới " +
        "điểm đầu đường thứ nhất và điểm cuối đường thứ hai, đây là điểm thứ 2")] GameObject objectTwoCurvePointOfRoad = null;

    [SerializeField, Tooltip("khoảng cách mà đường tạm sẽ tạo ra khi điểm đầu và điểm cuối di chuyển, tạo" +
        "chuyển động mượt hơn")] private int distanceToLerp = 5;

    [SerializeField] private bool canPutRoadInMap = true;

    [SerializeField, Tooltip("Snap vào grid của road")]
    private bool snapToGrid;

    public Transform startWidthRoad;
    public Transform endWidthRoad;

    private ObjectPool objectPool;

    public GameObject RoadPrefab { get => _roadPrefab; set => _roadPrefab = value; }


    public ObjectPool ObjectPool { get => objectPool; set => objectPool = value; }
    public GameObject MarkerPrefab { get => markerPrefab; set => markerPrefab = value; }
    public GameObject StartRoadTemporary { get => startRoadTemporary; set => startRoadTemporary = value; }
    public GameObject EndRoadTemporary { get => endRoadTemporary; set => endRoadTemporary = value; }
    public bool CanPutRoadInMap { get => canPutRoadInMap; set => canPutRoadInMap = value; }
    public GameObject TextPrefabs { get => textPrefabs; set => textPrefabs = value; }
    public GameObject FirstPointOfTwoRoad { get => firstPointOfTwoRoad; set => firstPointOfTwoRoad = value; }
    public GameObject ObjectTwoCurvePointOfRoad { get => objectTwoCurvePointOfRoad; set => objectTwoCurvePointOfRoad = value; }
    public GameObject ObjectOneCurvePointOfRoad { get => objectOneCurvePointOfRoad; set => objectOneCurvePointOfRoad = value; }
    public RoadBuild RoadBuild { get => roadBuild; set => roadBuild = value; }


    // Event
    [SerializeField] RoadBuild roadBuild;

    public void LoadData(GameData data)
    {
        // Instantiate contruction in data
        foreach (KeyValuePair<string, RoadData> kvp in data.roadDictionary)
        {
            string key = kvp.Key;
            RoadData value = kvp.Value;

            GameObject obj = Instantiate(_roadPrefab);

            GameObject roadLine = obj.transform.Find("RoadLine").gameObject;

            for(int i = 0; i< value.position.Count;i++)
            {
                GameObject segment = new GameObject();
                segment.transform.parent = roadLine.transform;
                segment.transform.position = value.position[i];
                segment.transform.rotation = Quaternion.Euler(value.rotation[i]);
                segment.transform.localScale = value.localScale[i];

                segment.AddComponent<CurvySplineSegment>();
                CurvySplineSegment curvySplineSegment = segment.GetComponent<CurvySplineSegment>();
                curvySplineSegment.AutoHandles = value.autoHandles[i];
                curvySplineSegment.HandleIn = value.handleIn[i];
                curvySplineSegment.HandleOut = value.handleOut[i];

                GameObject createMeshObj = obj.transform.Find("Create Mesh").gameObject;

                StartCoroutine(AddRoadCollisonComponant(createMeshObj, 0.5f));
            }

            roadLine.AddComponent<RoadSpline>();
            roadLine.GetComponent<RoadSpline>().Id = key;
        }
    }

    public void SaveData(GameData data)
    {
        foreach (string value in PlacementSystem.Instance.keyDeleteObject)
        {
            if (data.roadDictionary.ContainsKey(value))
                data.roadDictionary.Remove(value);
        }
    }

#pragma warning disable CS0114
    private void Awake()
    {
        base.Awake();
        this.gameObject.AddComponent<ObjectPool>();
        ObjectPool = this.GetComponent<ObjectPool>();
        ObjectPool.ObjectPrefab = MarkerPrefab;
        ObjectPool.InitializePool();
        FirstPointOfTwoRoad = null;
        //Event
    }
#pragma warning restore CS0114

    public void CreateStartPointRoad(string typeRoad)
    {
        RoadBuild.CreateStartPointRoad(RoadPrefab, typeRoad);
    }
    public void StopCreateStartPointRoad()
    {
        if (RoadBuild.typeRoad == "StraightRoad")
        {
            RoadBuild.StopCreateStartPointRoad();
        }
        else if (RoadBuild.typeRoad == "CurveRoad")
        {
            RoadBuild.CancelCurvedRoad();
        }
    }

    /// <summary>
    /// Cài đặt điểm đầu đường tạm
    /// </summary>
    /// <param name="startPoint"></param>
    public void SetStartRoadTemporary(Vector3 startPoint)
    {
        StartRoadTemporary.transform.position = startPoint;
    }
    /// <summary>
    /// Cài đặt điểm cuối đường tạm
    /// </summary>
    /// <param name="endPoint"></param>
    public void SetEndRoadTemporary(Vector3 endPoint)
    {
        //EndRoadTemporary.transform.position = endPoint;

        float totalDistance = Vector3.Distance(StartRoadTemporary.transform.position, endPoint) * Settings.ratioWorld;

        Vector3 direction = endPoint - StartRoadTemporary.transform.position;

        if(StartRoadTemporary)
        {
            EndRoadTemporary.transform.position = StartRoadTemporary.transform.position +
                direction.normalized * Mathf.FloorToInt((totalDistance / ( distanceToLerp))) * ((distanceToLerp) /Settings.ratioWorld)
                ;
        }
    }

    /// <summary>
    /// Xóa mesh cũ của Temporary
    /// </summary>
    public void DeleteMeshRoadTemporary()
    {
        foreach(Transform child in transform.GetChild(0).Find("Create Mesh"))
        {
            Destroy(child.gameObject);
        }
    }

    public float GetWidthRoad()
    {
        return Vector3.Distance(startWidthRoad.position, endWidthRoad.position);
    }

    private IEnumerator AddRoadCollisonComponant(GameObject createMeshObj, float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < createMeshObj.transform.childCount; i++)
        {
            if (!createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>())
                createMeshObj.transform.GetChild(i).AddComponent<RoadCollison>();
            if (createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>())
                createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>().Init();
        }
    }
}
