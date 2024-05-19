using UnityEngine;

public class ExportMesh : MonoBehaviour
{
    public string exportPath = "Assets/ExportedMesh.obj"; // Đường dẫn xuất file mesh

    private void Start()
    {
        Export();
    }

    // Hàm xuất mesh
    public void Export()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Mesh mesh = meshFilter.sharedMesh;
            ObjExporter.MeshToFile(mesh, exportPath); // Gọi hàm xuất file mesh
            Debug.Log("Mesh exported to: " + exportPath);
        }
        else
        {
            Debug.LogWarning("No mesh found to export.");
        }
    }
}
