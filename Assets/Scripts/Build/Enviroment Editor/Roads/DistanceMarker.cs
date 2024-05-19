using TMPro;
using UnityEngine;

public class DistanceMarker : MonoBehaviour
{
    public Transform startPoint; // Điểm bắt đầu của con đường
    public Transform endPoint; // Điểm kết thúc của con đường
    public GameObject curvePoint; // điểm làm mịn đường 
    public int markerSpacing = 1; // Khoảng cách giữa các cột đánh dấu

    private GameObject TextDistance;

    private float totalDistance; // Tổng khoảng cách giữa hai điểm
    public Vector3 lastPositionPoint;

    public float baseFontSize = 14f;
    public float distanceScaleFactor = 10f; // Yếu tố tỉ lệ khoảng cách

    public GameObject TextDistance1 { get => TextDistance; set => TextDistance = value; }

    public void Init()
    {
        GameObject markerPoint = new GameObject("MakerPoint");
        markerPoint.transform.transform.parent = this.transform.parent;
    }

    void Update()
    {
        if (lastPositionPoint != null && endPoint!=null && lastPositionPoint == endPoint.position)
            return;
    }

    /// <summary>
    /// Tính toán khoảng cách của road 
    /// </summary>
    public void CountDistance()
    {
        if (!PlacementSystem.Instance.mouseIndicator)
        {
            if (TextDistance1)
            {
                RoadController.Instance.ObjectPool.ReturnObjectToPool(TextDistance1);
            }
        }

        if (!RoadController.Instance.StartRoadTemporary || !RoadController.Instance.EndRoadTemporary)
            return;

        // Tính toán tổng khoảng cách giữa hai điểm
        if (RoadController.Instance.StartRoadTemporary && RoadController.Instance.EndRoadTemporary)
        {
            totalDistance = Vector3.Distance(RoadController.Instance.StartRoadTemporary.transform.position
                , RoadController.Instance.EndRoadTemporary.transform.position) * Settings.ratioWorld ;
        }


        GameObject marker = PlacementSystem.Instance.mouseIndicator;

        // Cập nhật vị trí của marker
        if(!marker.activeSelf)
        {
            marker.SetActive(true);
        }

        // tạo text đếm khoảng cách cho marker
        if (!TextDistance1)
        {
            TextDistance1 = RoadController.Instance.ObjectPool.GetObjectFromPool();
            TextDistance1.transform.localScale = new Vector3(.1f,.1f,.1f);
            //TextDistance1.transform.parent = marker.transform;
        }
        //else
        //TextDistance1 = marker.transform.GetChild(0).gameObject;

        if (TextDistance1)
        {
            if (!TextDistance1.activeSelf)
                TextDistance1.SetActive(true);

            marker.transform.localScale = new Vector3(1f / Settings.ratioWorld, 1f / Settings.ratioWorld, 1f / Settings.ratioWorld);

            // Tính toán khoảng cách giữa camera và đối tượng chứa text
            float distanceToCamera = Vector3.Distance(marker.transform.position, Camera.main.transform.position);

            // Tính toán font size mới dựa trên khoảng cách và yếu tố tỉ lệ
            float newSize = baseFontSize * (1 + distanceToCamera / distanceScaleFactor);

            UnityEngine.Color color;
            if (ColorUtility.TryParseHtmlString("#00AAFF", out color))
            {
                // Đặt màu cho Material
                TextDistance1.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().color = color;
            }

            TextDistance1.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().fontSize = newSize;

            TextDistance1.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{(totalDistance).ToString("F2")}m";

            // Tính toán vị trí của text
            Vector3 textPosition = new Vector3();
            if (marker.GetComponent<SphereCollider>())
                textPosition = marker.transform.position + (Vector3.up * marker.GetComponent<SphereCollider>().radius / 2f) / Settings.ratioWorld;

            // text hướng về mắt 
            Vector3 directionVec = Camera.main.transform.forward;

            TextDistance1.transform.position = textPosition;
            TextDistance1.transform.rotation = Quaternion.LookRotation(directionVec);
        }
    }

    /// <summary>
    /// Tạo road mỗi lần x mét
    /// </summary>
    public void CreateMarkersAlongPath()
    {
        if(startPoint && endPoint)
        {
            float totalDistance = Vector3.Distance(startPoint.transform.position
                , endPoint.transform.position) * Settings.ratioWorld;

            // Tính toán số lượng cột đánh dấu cần tạo
            int numberOfMarkers = Mathf.FloorToInt(totalDistance / (markerSpacing));

            Vector3 direction = endPoint.transform.position - startPoint.transform.position;

            for (int i = 0; i <= numberOfMarkers; i++)
            {
                GameObject pointOnRoad = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                pointOnRoad.name = "point " + i+1;
                pointOnRoad.transform.parent = transform;

                Destroy(pointOnRoad.GetComponent<MeshFilter>());
                Destroy(pointOnRoad.GetComponent<MeshRenderer>());

                pointOnRoad.transform.localScale = new Vector3(.1f,.1f,.1f);

                pointOnRoad.transform.position = startPoint.transform.position +
                    direction.normalized * (markerSpacing/Settings.ratioWorld) * i; 
                    ;

                // điểm trên đường chứa gốc tọa độ song song với Road
                Vector3 pointInLineOfCoordinateOriginParallelWithRoad = Vector3.zero +
                    (endPoint.transform.position - startPoint.transform.position).normalized;
            }
        }
    }
}