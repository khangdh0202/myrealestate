using UnityEngine;

public class RoadMeshUpdater : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    // Điểm đầu và cuối của đường
    public Vector3 startPoint;
    public Vector3 endPoint;

    // Khởi tạo
    void Start()
    {
        // Lấy MeshFilter component
        meshFilter = GetComponent<MeshFilter>();

        // Tạo một mesh mới
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Cập nhật mesh ban đầu dựa trên điểm đầu và cuối
        UpdateMesh();
    }

    // Cập nhật mesh dựa trên điểm đầu và cuối
    void UpdateMesh()
    {
        // Tạo vertices
        Vector3[] vertices = new Vector3[2];
        vertices[0] = startPoint;
        vertices[1] = endPoint;

        // Tạo triangles (chú ý rằng đây là một đoạn đường thẳng nên chỉ cần 2 vertices)
        int[] triangles = new int[] { 0, 1 };

        // Gán vertices và triangles vào mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Tính toán normals để hiển thị đúng
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        // Nếu startPoint hoặc endPoint thay đổi, cập nhật mesh
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Ví dụ: Randomize điểm đầu và cuối cho mục đích demo
            startPoint = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            endPoint = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

            // Cập nhật mesh
            UpdateMesh();
        }
    }
}
