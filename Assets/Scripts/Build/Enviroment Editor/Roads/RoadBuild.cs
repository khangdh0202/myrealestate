using FluffyUnderware.Curvy;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoadBuild : MonoBehaviour, IInputBuilding
{
    private void FixedUpdate()
    {
        
        if (PlacementSystem.Instance.TypeOfMouseIndicator == "Road" && RoadController.Instance.FirstPointOfTwoRoad != null)
        {
            
            if (typeRoad == "CurveRoad" && objectMiddleOfCurvedRoad)
            {
                if (RoadController.Instance.FirstPointOfTwoRoad)
                    AngleOfRoads(RoadController.Instance.FirstPointOfTwoRoad.transform.position, PointBOrinalOfCurveRoad, ref PlacementSystem.Instance.mouseIndicator);
                UpdateCurveRoad();
                RoadController.Instance.SetEndRoadTemporary(PlacementSystem.Instance.mouseIndicator.transform.position);
            }

            if (typeRoad == "StraightRoad")
            {
                if (RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>() && RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint != null)
                    AngleOfRoads(RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint.gameObject.transform.position, RoadController.Instance.StartRoadTemporary.transform.position, ref PlacementSystem.Instance.mouseIndicator);

                if (RoadController.Instance.StartRoadTemporary)
                {
                    if (Vector3.Distance(RoadController.Instance.StartRoadTemporary.transform.position,
                    PlacementSystem.Instance.mouseIndicator.transform.position) >= 1f
                    )
                    {
                        RoadController.Instance.CanPutRoadInMap = true;
                        RoadController.Instance.SetEndRoadTemporary(PlacementSystem.Instance.mouseIndicator.transform.position);
                    }
                    else
                    {
                        RoadController.Instance.CanPutRoadInMap = false;
                        RoadController.Instance.SetEndRoadTemporary(RoadController.Instance.StartRoadTemporary.transform.position);
                    }
                }
                if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>())
                {
                    PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().CountDistance();
                }
            }
        }
        //CheckMeshCollisonInMesh();
        UpdatePositionCheckCollisionWithRoad();
    }


    int dem = 0;
    public void PlaceItem()
    {
        if (PlacementSystem.Instance.TypeOfMouseIndicator == "Road" && RoadController.Instance.CanPutRoadInMap)
        {
            if (typeRoad == "CurveRoad")
            {
                CurveRoad();
                return;
            }
            else if (typeRoad == "StraightRoad")
            {
                StraightRoad();
                return;
            }
        }
    }

    GameObject textDistance;
    float baseFontSize = 14f, distanceScaleFactor = 10f;
    public GameObject objectMidPointOfRoadCurvy,fixedCurvePoint;
    public float valueToCurvy;
    float angle;

    GameObject objectCheckBound;
    private void CheckCollisionWithRoad()
    {
        if (objectCheckBound)
            Destroy(objectCheckBound);

        if (!RoadController.Instance.ObjectTwoCurvePointOfRoad || !PlacementSystem.Instance.mouseIndicator)
            return;

        objectCheckBound = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(objectCheckBound.GetComponent<MeshFilter>());
        Destroy(objectCheckBound.GetComponent<MeshRenderer>());
        //Destroy(objectCheckBound.GetComponent<MeshFilter>());
        objectCheckBound.AddComponent<Rigidbody>();
        objectCheckBound.GetComponent<Collider>().isTrigger = true;
        objectCheckBound.GetComponent<Rigidbody>().useGravity = false;
        objectCheckBound.AddComponent<CheckCanPutRoadCollision>();
        objectCheckBound.transform.position = RoadController.Instance.StartRoadTemporary.transform.position;

        Vector3 direction = PlacementSystem.Instance.mouseIndicator.transform.position - RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.position;

        objectCheckBound.transform.position = RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.position + direction.normalized * (1.5f);

        objectCheckBound.transform.parent = RoadController.Instance.StartRoadTemporary.transform.parent;
    }

    private void CheckMeshCollisonInMesh()
    {
        GameObject createMeshObj = PlacementSystem.Instance.mouseIndicator.transform.parent.parent.Find("Create Mesh").gameObject;

        StartCoroutine(AddRoadCollisonComponant(createMeshObj, 2f));
    }

    private void UpdatePositionCheckCollisionWithRoad()
    {
        if (!objectCheckBound)
            return;
        Vector3 direction = PlacementSystem.Instance.mouseIndicator.transform.position - RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.position;

        objectCheckBound.transform.position = RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.position + direction.normalized * (1.5f);
    }

    private void StraightRoad()
    {
        // check Angle lần cuối 
        if (RoadController.Instance.FirstPointOfTwoRoad != null && RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint != null)
            AngleOfRoads(RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint.gameObject.transform.position, RoadController.Instance.StartRoadTemporary.transform.position, ref PlacementSystem.Instance.mouseIndicator);


        // Save point of road (start and end point)
        if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>() == null)
        {
            //Debug.Log($"1");
            PlacementSystem.Instance.mouseIndicator.AddComponent<DistanceMarker>();
            PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().endPoint = PlacementSystem.Instance.mouseIndicator.transform;

        }

        Vector3 diretionToCurve = Vector3.zero;

        RoadController.Instance.ObjectOneCurvePointOfRoad = null;

        if (RoadController.Instance.FirstPointOfTwoRoad != null)
        {
            //RoadController.Instance.FirstPointOfTwoRoad.transform.parent = null;

            //Debug.Log($"2");
            RoadController.Instance.ObjectOneCurvePointOfRoad = CreateCurvePointOfRoad(1);
            Vector3 direction = RoadController.Instance.FirstPointOfTwoRoad.transform.position - RoadController.Instance.ObjectOneCurvePointOfRoad.transform.position;

            RoadController.Instance.ObjectOneCurvePointOfRoad.transform.Translate(direction.normalized * (4f / Settings.ratioWorld));

            RoadController.Instance.ObjectOneCurvePointOfRoad.AddComponent<CurvySplineSegment>();
            CurvySplineSegment curvySplineSegment = RoadController.Instance.ObjectOneCurvePointOfRoad.GetComponent<CurvySplineSegment>();
            curvySplineSegment.AutoHandles = false;
            curvySplineSegment.HandleIn = new Vector3(0, 0, 0);
            curvySplineSegment.HandleOut = new Vector3(0, 0, 0);

            if (RoadController.Instance.ObjectTwoCurvePointOfRoad != null)
            {
                //Debug.Log($"3");
                Vector3 direction2 = RoadController.Instance.EndRoadTemporary.transform.position - RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.position;

                RoadController.Instance.ObjectTwoCurvePointOfRoad.transform.Translate(direction2.normalized * (4f / Settings.ratioWorld));

                RoadController.Instance.ObjectTwoCurvePointOfRoad.AddComponent<CurvySplineSegment>();
                CurvySplineSegment curvySplineSegment1 = RoadController.Instance.ObjectTwoCurvePointOfRoad.GetComponent<CurvySplineSegment>();
                curvySplineSegment1.AutoHandles = false;
                curvySplineSegment1.HandleIn = new Vector3(0, 0, 0);
                curvySplineSegment1.HandleOut = new Vector3(0, 0, 0);

                RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvePointOfEndRoad>().secondPoint = RoadController.Instance.ObjectTwoCurvePointOfRoad;
                //diretionToCurve = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvePointOfEndRoad>().firstPoint.transform.position, RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvePointOfEndRoad>().secondPoint.transform.position, RoadController.Instance.FirstPointOfTwoRoad.transform.position) - RoadController.Instance.FirstPointOfTwoRoad.transform.position;

            }
        }

        // lưu lại điểm đầu cảu con đường trước đó
        if (RoadController.Instance.FirstPointOfTwoRoad == null)
        {
            //Debug.Log($"4");
            RoadController.Instance.FirstPointOfTwoRoad = PlacementSystem.Instance.mouseIndicator;
        }
        else if (RoadController.Instance.FirstPointOfTwoRoad != null && PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().startPoint != null)
        {
            //Debug.Log($"5");
            RoadController.Instance.FirstPointOfTwoRoad = PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().endPoint.gameObject;
            //RoadController.Instance.FirstPointOfTwoRoad.transform.position = RoadController.Instance.EndRoadTemporary.transform.position;
        }

        // trả các các textPrefabs về pool
        /*for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
        {
            RoadController.Instance.ObjectPool.ReturnObjectToPool(PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject);
        }*/
        if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>())
        {
            if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().TextDistance1)
            {
                RoadController.Instance.ObjectPool.ReturnObjectToPool(PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().TextDistance1);
            }
        }

        GameObject finalPoint = Instantiate(PlacementSystem.Instance.mouseIndicator);
        finalPoint.SetActive(false);

        finalPoint.transform.parent = PlacementSystem.Instance.mouseIndicator.transform.parent;
        finalPoint.transform.localPosition = PlacementSystem.Instance.mouseIndicator.transform.localPosition;

        if (RoadController.Instance.FirstPointOfTwoRoad != null)
        {
            //Debug.Log($"6");
            finalPoint.GetComponent<DistanceMarker>().startPoint = RoadController.Instance.FirstPointOfTwoRoad.transform;
        }

        PlacementSystem.Instance.mouseIndicator.AddComponent<CurvySplineSegment>();
        PlacementSystem.Instance.mouseIndicator.AddComponent<MetaCGOptions>();

        if (PlacementSystem.Instance.mouseIndicator.GetComponent<CurvySplineSegment>() != null)
        {
            //Debug.Log($"7");
            CurvySplineSegment curvySplineSegment = PlacementSystem.Instance.mouseIndicator.GetComponent<CurvySplineSegment>();
            curvySplineSegment.AutoHandles = false;
            curvySplineSegment.HandleIn = new Vector3(0, 0, 0);
            curvySplineSegment.HandleOut = new Vector3(0, 0, 0);
        }

        // Xóa mesh đường tạm
        RoadController.Instance.DeleteMeshRoadTemporary();


        RoadController.Instance.SetStartRoadTemporary(PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().endPoint.position);
        RoadController.Instance.SetEndRoadTemporary(PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().endPoint.position);

        if (RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>().Connection)
        {
            //Debug.Log($"8");
            RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>().Connection.Delete();
        }
        if (RoadController.Instance.FirstPointOfTwoRoad != null)
        {
            //Debug.Log($"9");
            if (RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvySplineSegment>().Connection)
                RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvySplineSegment>().Connection.Delete();
        }

        if (CurvyGlobalManager.Instance.gameObject)
        {
            //Debug.Log($"10");
            GameObject connection = new GameObject("connection");
            connection.transform.parent = CurvyGlobalManager.Instance.gameObject.transform;
            connection.AddComponent<CurvyConnection>();
            CurvyConnection curvyConnection = connection.GetComponent<CurvyConnection>();

            curvyConnection.AddControlPoints(PlacementSystem.Instance.mouseIndicator.GetComponent<CurvySplineSegment>(), RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>());

            RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>().ConnectionSyncPosition = true;
            PlacementSystem.Instance.mouseIndicator.GetComponent<CurvySplineSegment>().ConnectionSyncPosition = true;
        }

        finalPoint.transform.parent.GetComponent<DistanceMarker>().startPoint = PlacementSystem.Instance.mouseIndicator.transform;
        finalPoint.transform.parent.GetComponent<DistanceMarker>().endPoint = finalPoint.transform;
        Destroy(PlacementSystem.Instance.mouseIndicator.GetComponent<MeshRenderer>());
        Destroy(PlacementSystem.Instance.mouseIndicator.GetComponent<MeshFilter>());
        Destroy(PlacementSystem.Instance.mouseIndicator.GetComponent<Rigidbody>());
        Destroy(PlacementSystem.Instance.mouseIndicator.GetComponent<CheckCanPutRoadCollision>());


        RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().curvePoint = CreateCurvePointOfRoad(0);


        if (RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().curvePoint != null)
        {
            RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().curvePoint.name = "CCC" + dem;
            dem++;
        }

        RoadController.Instance.FirstPointOfTwoRoad.AddComponent<CurvePointOfEndRoad>();
        RoadController.Instance.FirstPointOfTwoRoad.GetComponent<CurvePointOfEndRoad>().firstPoint = RoadController.Instance.ObjectOneCurvePointOfRoad;

        //Debug.Log($"{firstPointOfRoad} - {objectOneCurvePointOfRoad}");
        if (RoadController.Instance.ObjectOneCurvePointOfRoad != null)
        {
            RoadController.Instance.ObjectTwoCurvePointOfRoad = CreateCurvePointOfRoad(1);
            //Debug.Log($"11");

        }

        // tạo 1 box check xem có đụng road nào ở gốc road không
        CheckCollisionWithRoad();

        if (RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint != null)
        {
            CurvePointOfEndRoad curvePointOfEndRoad = RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint.gameObject.GetComponent<CurvePointOfEndRoad>();
            
            if (curvePointOfEndRoad.firstPoint && curvePointOfEndRoad.secondPoint)
            {
                Vector3 middlePoint = (curvePointOfEndRoad.firstPoint.transform.position + curvePointOfEndRoad.secondPoint.transform.position) / 2f;
                RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint.transform.position = (RoadController.Instance.FirstPointOfTwoRoad.GetComponent<DistanceMarker>().startPoint.transform.position + middlePoint) / 2f;
            }
        }

        if (objectMidPointOfRoadCurvy)
        {
            Destroy(PlacementSystem.Instance.mouseIndicator.GetComponent<CurvePointOfEndRoad>().firstPoint);

            GameObject obj = Instantiate(objectMidPointOfRoadCurvy);

            int index = (PlacementSystem.Instance.mouseIndicator.transform.GetSiblingIndex() + RoadController.Instance.FirstPointOfTwoRoad.transform.GetSiblingIndex()) / 2;

            obj.transform.parent = PlacementSystem.Instance.mouseIndicator.transform.parent;
            obj.transform.position = objectMidPointOfRoadCurvy.transform.position;

            obj.transform.SetSiblingIndex(index - 1); 
        }

        //PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().CreateMarkersAlongPath();

        PlacementSystem.Instance.mouseIndicator = finalPoint;

        CheckMeshCollisonInMesh();

        // sound
        PlacementSystem.Instance.Sound();

        // finalPoint
        PlacementSystem.Instance.mouseIndicator.SetActive(false);
    }

    private GameObject objectMiddleOfCurvedRoad = null;
    [SerializeField]private Vector3 PointBOrinalOfCurveRoad;

    private void UpdateCurveRoad()
    {
        if (objectMiddleOfCurvedRoad)
        {
            //Vector3 pointToMidPoint = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(RoadController.Instance.EndRoadTemporary.transform.position, RoadController.Instance.StartRoadTemporary.transform.position, PointBOrinalOfCurveRoad);

            Vector3 middlePoint = (RoadController.Instance.StartRoadTemporary.transform.position + RoadController.Instance.EndRoadTemporary.transform.position) / 2f;

            objectMiddleOfCurvedRoad.transform.position = (PointBOrinalOfCurveRoad + middlePoint) / 2f;
            testpoint1 = PointBOrinalOfCurveRoad;
        }
    }
    /// <summary>
    /// Hàm đường cong
    /// </summary>
    private void CurveRoad()
    {
        if (objectMiddleOfCurvedRoad != null)
        {
            GameObject roadLine = PlacementSystem.Instance.mouseIndicator.transform.parent.gameObject;

            for(int i = 0; i < roadLine.transform.childCount; i++)
            {
                Destroy(roadLine.transform.GetChild(i).gameObject.GetComponent<MeshFilter>());
                Destroy(roadLine.transform.GetChild(i).gameObject.GetComponent<Collider>());
                Destroy(roadLine.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>());
                Destroy(roadLine.transform.GetChild(i).gameObject.GetComponent<Rigidbody>());
                Destroy(roadLine.transform.GetChild(i).gameObject.GetComponent<CheckCanPutRoadCollision>());
                roadLine.transform.GetChild(i).gameObject.AddComponent<CurvySplineSegment>();

                if (i == 1)
                {
                    roadLine.transform.GetChild(i).gameObject.transform.position = objectMiddleOfCurvedRoad.transform.position;
                }

                if(i == 0 || i ==  roadLine.transform.childCount - 1)
                {
                    if (roadLine.transform.GetChild(i).gameObject.GetComponent<CurvySplineSegment>() != null)
                    {
                        //Debug.Log($"7");
                        CurvySplineSegment curvySplineSegment = roadLine.transform.GetChild(i).gameObject.GetComponent<CurvySplineSegment>();
                        curvySplineSegment.AutoHandles = false;
                        curvySplineSegment.HandleIn = new Vector3(0, 0, 0);
                        curvySplineSegment.HandleOut = new Vector3(0, 0, 0);
                    }
                }
            }

            if(!roadLine.GetComponent<RoadSpline>())
                roadLine.AddComponent<RoadSpline>();

            roadLine.SetActive(false);
            roadLine.SetActive(true);


            CancelCurvedRoad();

            //Sound
            PlacementSystem.Instance.Sound();

            return;
        }
        // lưu lại điểm đầu cảu con đường trước đó
        if (RoadController.Instance.FirstPointOfTwoRoad == null)
        {
            //Debug.Log($"4");

            RoadController.Instance.SetStartRoadTemporary(PlacementSystem.Instance.mouseIndicator.transform.position);

            RoadController.Instance.FirstPointOfTwoRoad = PlacementSystem.Instance.mouseIndicator;

            GameObject obj;

            obj = Instantiate(PlacementSystem.Instance.mouseIndicator);

            obj.transform.parent = PlacementSystem.Instance.mouseIndicator.transform.parent;

            PlacementSystem.Instance.mouseIndicator = obj;
        }
        else if (RoadController.Instance.FirstPointOfTwoRoad != null)
        {
            PointBOrinalOfCurveRoad = PlacementSystem.Instance.mouseIndicator.transform.position;
            if (objectMiddleOfCurvedRoad == null)
            {
                // Tính toán index giữa child1 và child2
                int index = 1;

                Transform parentObj = RoadController.Instance.StartRoadTemporary.transform.parent;

                // Instantiate và thêm đối tượng con mới vào giữa
                objectMiddleOfCurvedRoad = Instantiate(new GameObject(), parentObj);
                objectMiddleOfCurvedRoad.transform.SetSiblingIndex(index);
                // Tạo một midPoint tại đường tạm
                objectMiddleOfCurvedRoad.transform.parent = RoadController.Instance.StartRoadTemporary.transform.parent;

            }

            if (!objectMiddleOfCurvedRoad.GetComponent<CurvySplineSegment>())
                objectMiddleOfCurvedRoad.AddComponent<CurvySplineSegment>();

            GameObject obj;

            obj = Instantiate(PlacementSystem.Instance.mouseIndicator);

            obj.transform.parent = PlacementSystem.Instance.mouseIndicator.transform.parent;

            PlacementSystem.Instance.mouseIndicator = obj;
        }
    }

    public void CancelCurvedRoad()
    {
        Destroy(objectMiddleOfCurvedRoad);

        RoadController.Instance.FirstPointOfTwoRoad = null;

        if (textDistance)
            RoadController.Instance.ObjectPool.ReturnObjectToPool(textDistance);

        if (PlacementSystem.Instance.mouseIndicator.transform.parent.childCount < 3)
        {
            Destroy(PlacementSystem.Instance.mouseIndicator.transform.parent.parent.gameObject);

            if (PlacementSystem.Instance.mouseIndicator)
                Destroy(PlacementSystem.Instance.mouseIndicator);
            return;
        }
        GameObject createMeshObj = PlacementSystem.Instance.mouseIndicator.transform.parent.parent.Find("Create Mesh").gameObject;

        StartCoroutine(AddRoadCollisonComponant(createMeshObj, 0.5f));

        PlacementSystem.Instance.TypeOfMouseIndicator = "null";

        RoadController.Instance.SetStartRoadTemporary(new Vector3());
        RoadController.Instance.SetEndRoadTemporary(new Vector3());

        PlacementSystem.Instance.mouseIndicator = null;
    }

    Vector3 testpoint1, testpoint2;
    Vector3 orinalPointB;
    /// <summary>
    /// Hàm tính góc cho đường
    /// </summary>
    private void AngleOfRoads(Vector3 objectOneOfRoad, Vector3 objectTwoOfRoad, ref GameObject mouseIndicator)
    {
        if (objectOneOfRoad == null || objectTwoOfRoad == null || mouseIndicator == null)
            return;

        if (!SettingManager.Instance.SnapToAngle)
            return;

        if (Settings.RoadSnapToRoad)
            return;

        if (!PlacementSystem.Instance.mouseIndicator)
        {
            if (textDistance)
            {
                RoadController.Instance.ObjectPool.ReturnObjectToPool(textDistance);
            }
        }

        if (textDistance == null)
        {
            textDistance = RoadController.Instance.ObjectPool.GetObjectFromPool();
            textDistance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            //textDistance.transform.parent = mouseIndicator.transform;
        }

        if (textDistance != null)
        {
            //float angle;

            angle = Vector3.Angle(objectTwoOfRoad - objectOneOfRoad, objectTwoOfRoad - mouseIndicator.transform.position);

            if (angle >= 85 && angle <= 95)
            {
                Vector3 value = new Vector3();

                Vector3 direction = objectTwoOfRoad - objectOneOfRoad;

                // Điểm từ mouseIndicator đến direction, song song đoạn đường 1 
                value = mouseIndicator.transform.position + direction;

                mouseIndicator.transform.position = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(mouseIndicator.transform.position, value, objectTwoOfRoad);

                Vector3 direction1 = objectTwoOfRoad - objectOneOfRoad;
                Vector3 direction2 = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(mouseIndicator.transform.position, value, objectTwoOfRoad) - objectTwoOfRoad;

                float cosAngle = Vector3.Dot(direction1.normalized, direction2.normalized);
                angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;

            }
            else if (angle >= 175 || angle <= 5)
            {
                Vector3 direction1 = objectTwoOfRoad - objectOneOfRoad;
                Vector3 direction2 = objectTwoOfRoad - mouseIndicator.transform.position;

                
                mouseIndicator.transform.position = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(objectOneOfRoad, objectTwoOfRoad, mouseIndicator.transform.position);

                direction2 = objectTwoOfRoad - mouseIndicator.transform.position;

                float cosAngle = Vector3.Dot(direction1.normalized, direction2.normalized);
                angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
                /*if (angle >= 175)
                    angle = 180;
                else
                    angle = 0;*/
            }
            if (typeRoad == "StraightRoad")
            {
                if (angle >= 0 && angle <= 45)
                {
                    float distance = Vector3.Distance(objectTwoOfRoad, mouseIndicator.transform.position);

                    if (objectMidPointOfRoadCurvy == null)
                    {

                        // Tính toán index giữa child1 và child2
                        int index = 1;

                        Transform parentObj = RoadController.Instance.StartRoadTemporary.transform.parent;

                        // Instantiate và thêm đối tượng con mới vào giữa
                        objectMidPointOfRoadCurvy = Instantiate(new GameObject(), parentObj);
                        objectMidPointOfRoadCurvy.transform.SetSiblingIndex(index);
                        // Tạo một midPoint tại đường tạm
                        objectMidPointOfRoadCurvy.transform.parent = RoadController.Instance.StartRoadTemporary.transform.parent;


                        if (!objectMidPointOfRoadCurvy.GetComponent<CurvySplineSegment>())
                            objectMidPointOfRoadCurvy.AddComponent<CurvySplineSegment>();

                        Vector3 position = objectTwoOfRoad;

                        objectMidPointOfRoadCurvy.transform.position = position;
                        objectMidPointOfRoadCurvy.transform.Translate((mouseIndicator.transform.position - objectTwoOfRoad).normalized
                            * ((distance * 70f) / 100f)
                            , Space.World);

                        orinalPointB = objectMidPointOfRoadCurvy.transform.position;
                        test3 = orinalPointB;
                    }
                    if (objectMidPointOfRoadCurvy)
                    {

                        Vector3 middlePoint = KhangLibrary.LineIntersection3D.GetPerpendicularPoint(RoadController.Instance.StartRoadTemporary.transform.position, RoadController.Instance.EndRoadTemporary.transform.position, orinalPointB);

                        Vector3 directionPointBAndMidPoint = orinalPointB - objectTwoOfRoad;

                        orinalPointB = objectTwoOfRoad + directionPointBAndMidPoint.normalized * (70 * distance / 100);
                        test3 = orinalPointB;
                        objectMidPointOfRoadCurvy.transform.position = (orinalPointB + middlePoint) / 2f;
                        test4 = objectMidPointOfRoadCurvy.transform.position;

                    }
                }
                else
                {
                    Destroy(objectMidPointOfRoadCurvy);
                }
            }
            // Tính toán khoảng cách giữa camera và đối tượng chứa text
            float distanceToCamera = Vector3.Distance(mouseIndicator.transform.position, Camera.main.transform.position);

            // Tính toán font size mới dựa trên khoảng cách và yếu tố tỉ lệ
            float newSize = baseFontSize * (1 + distanceToCamera / distanceScaleFactor);

            Color color;
            if (UnityEngine.ColorUtility.TryParseHtmlString("#00AAFF", out color))
            {
                // Đặt màu cho Material
                textDistance.transform.GetChild(0).GetComponent<TextMeshPro>().color = color;
            }

            textDistance.transform.GetChild(0).GetComponent<TextMeshPro>().fontSize = newSize;

            textDistance.transform.GetChild(0).GetComponent<TextMeshPro>().text = string.Format("{0}°", angle);

            Vector3 textPosition = new Vector3();

            // Tính toán vị trí của text
            if (mouseIndicator.gameObject.GetComponent<SphereCollider>())
                    textPosition = mouseIndicator.transform.position + (Vector3.up * mouseIndicator.GetComponent<SphereCollider>().radius) / Settings.ratioWorld +
                                                                       ((-RoadController.Instance.StartRoadTemporary.transform.position + mouseIndicator.transform.position).normalized * ((mouseIndicator.GetComponent<SphereCollider>().radius + mouseIndicator.GetComponent<SphereCollider>().radius)/2)) / Settings.ratioWorld;

            // text hướng về mắt 
            Vector3 directionVec = Camera.main.transform.forward;

            textDistance.transform.position = textPosition;
            textDistance.transform.rotation = Quaternion.LookRotation(directionVec);

            if (!textDistance.activeSelf)
            {
                textDistance.SetActive(true);
            }
        }
    }

    public string typeRoad = "";
    /// <summary>
    /// Tại 1 object trên mouseIndicator, điểm đặt trên map đầu tiên là điểm bắt đầu của road
    /// </summary>
    /// <param name="roadPrefab"></param>
    public void CreateStartPointRoad(GameObject roadPrefab, string typeRoad)
    {
#pragma warning disable
        GameObject _roadPrefab = Instantiate(roadPrefab);
#pragma warning restore
        _roadPrefab.transform.Find("RoadLine").AddComponent<DistanceMarker>();

        GameObject firstPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //Destroy(firstPoint.GetComponent<Collider>());
        //firstPoint.AddComponent<BoxCollider>();
        firstPoint.AddComponent<Rigidbody>();
        //firstPoint.tag = "RoadCheck";

        firstPoint.GetComponent<Collider>().isTrigger = true;
        firstPoint.GetComponent<Rigidbody>().useGravity = false;


        firstPoint.AddComponent<CheckCanPutRoadCollision>();

        firstPoint.transform.localScale = new Vector3(1f / Settings.ratioWorld, 1f / Settings.ratioWorld, 1f / Settings.ratioWorld);

        firstPoint.transform.parent = _roadPrefab.transform.Find("RoadLine");

        // thiết lập lại size box cho điểm cuối của đường tạm

        int childCount = roadPrefab.transform.Find("RoadShape").childCount;

        float value = Vector3.Distance(_roadPrefab.transform.Find("RoadShape").GetChild(0).position, _roadPrefab.transform.Find("RoadShape").GetChild(childCount - 1).position);

        if(firstPoint.gameObject.GetComponent<SphereCollider>())
            firstPoint.gameObject.GetComponent<SphereCollider>().radius = (value / 2) * Settings.ratioWorld;

        // tạo biến chứa thông tin khoảng cách trên map
        _roadPrefab.transform.Find("RoadLine").GetComponent<DistanceMarker>().startPoint = firstPoint.transform;
        _roadPrefab.transform.Find("RoadLine").GetComponent<DistanceMarker>().Init();

        PlacementSystem.Instance.TypeOfMouseIndicator = "Road";
        this.typeRoad = typeRoad;
        RoadController.Instance.CanPutRoadInMap = true;

        PlacementSystem.Instance.mouseIndicator = _roadPrefab.transform.Find("RoadLine").GetChild(0).gameObject;

        PlacementSystem.Instance.haveObjectInMouseIndicator = true;
    }


    /// <summary>
    /// Dừng việc tạo road 
    /// </summary>
    public void StopCreateStartPointRoad()
    {
        RoadController.Instance.FirstPointOfTwoRoad = null;
        
        PlacementSystem.Instance.mouseIndicator.transform.parent.gameObject.SetActive(false);
        PlacementSystem.Instance.mouseIndicator.transform.parent.gameObject.SetActive(true);

        for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.parent.childCount; i++)
        {
            if (!PlacementSystem.Instance.mouseIndicator.transform.parent.GetChild(i).gameObject.GetComponent<CurvySplineSegment>())
            {
                Destroy(PlacementSystem.Instance.mouseIndicator.transform.parent.GetChild(i).gameObject);
            }
        }

        GameObject createMeshObj = PlacementSystem.Instance.mouseIndicator.transform.parent.parent.Find("Create Mesh").gameObject;

        StartCoroutine(AddRoadCollisonComponant(createMeshObj , 0.5f));

        if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>())
        {
            if (PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().TextDistance1)
            {
                RoadController.Instance.ObjectPool.ReturnObjectToPool(PlacementSystem.Instance.mouseIndicator.GetComponent<DistanceMarker>().TextDistance1);
            }
        }

        if(textDistance)
            RoadController.Instance.ObjectPool.ReturnObjectToPool(textDistance);

        PlacementSystem.Instance.TypeOfMouseIndicator = "null";

        RoadController.Instance.SetStartRoadTemporary(new Vector3());
        RoadController.Instance.SetEndRoadTemporary(new Vector3());

        Destroy(RoadController.Instance.ObjectTwoCurvePointOfRoad);
        
        if (RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>().Connection)
        {
            RoadController.Instance.StartRoadTemporary.GetComponent<CurvySplineSegment>().Connection.Delete();
        }
        //Debug.Log($"{RoadController.Instance.gameObject.transform.transform.GetChild(0).name}");
        foreach (Transform child in RoadController.Instance.gameObject.transform.GetChild(0).Find("RoadLine"))
        {
            if (child.gameObject != RoadController.Instance.StartRoadTemporary && child.gameObject != RoadController.Instance.EndRoadTemporary)
                Destroy(child.gameObject);
        }
        GameObject parent = PlacementSystem.Instance.mouseIndicator.transform.parent.gameObject;

        if (!parent.GetComponent<RoadSpline>())
            parent.AddComponent<RoadSpline>();

        //Debug.Log("1"+parent.name);

        Destroy(PlacementSystem.Instance.mouseIndicator);
        //Debug.Log("2" + parent.transform.childCount);
        if (parent.transform.childCount <= 3)
        {
            //Debug.Log("3" + parent.transform.parent.name);
            Destroy(parent.transform.parent.gameObject);
        }

        PlacementSystem.Instance.CancelSnap();
    }

    private IEnumerator AddRoadCollisonComponant(GameObject createMeshObj , float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < createMeshObj.transform.childCount; i++)
        {
            if(!createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>())
                createMeshObj.transform.GetChild(i).AddComponent<RoadCollison>();
            if (createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>())
                createMeshObj.transform.GetChild(i).GetComponent<RoadCollison>().Init();
        }
    }

    /// <summary>
    /// Tạo ra điểm Curve Point cho 1 điểm của road <see cref="RoadController.ObjectTwoCurvePointOfRoad"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public GameObject CreateCurvePointOfRoad(int value)
    {
        int index = (PlacementSystem.Instance.mouseIndicator.transform.GetSiblingIndex() + RoadController.Instance.FirstPointOfTwoRoad.transform.GetSiblingIndex()) / 2;

        // tạo đường cong hoặc không
        GameObject obj;

        obj = new GameObject();

        obj.transform.parent = PlacementSystem.Instance.mouseIndicator.transform.parent;
        obj.transform.SetSiblingIndex(index + value); // 1 => trước mouseIndicator

        obj.transform.position = PlacementSystem.Instance.mouseIndicator.transform.position;

        return obj;
    }

    public void ReSizeItem(float y)
    {
        throw new System.NotImplementedException();
    }

    public void ReRotateItem(float x, float y)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteItem(GameObject obj)
    {
        Debug.Log("DeleteItem Road");
        // Delete in database
        PlacementSystem.Instance.keyDeleteObject.Add(obj.transform.parent.parent.Find("RoadLine").GetComponent<RoadSpline>().Id);
        // Xóa luôn đoạn đường dài
        Destroy(obj.transform.parent.parent.gameObject);
        // TODO - Xóa từng đoạn đường nhỏ
        //Destroy(obj); // chưa lưu trên .save

    }

    Vector3 test3, test4;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(testpoint1,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(testpoint2, 0.2f);
        Gizmos.DrawSphere(test3, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(test4, 0.2f);
    }
}
