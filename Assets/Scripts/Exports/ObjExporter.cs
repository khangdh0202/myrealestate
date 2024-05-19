using System.IO;
using UnityEngine;

public static class ObjExporter
{
    public static void MeshToFile(Mesh mesh, string filename)
    {
        string directory = Path.GetDirectoryName(filename);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString(mesh));
        }
    }

    public static string MeshToString(Mesh mesh)
    {
        // TODO: Convert mesh to string format (e.g., Wavefront .obj format)
        return "mesh.obj"; // Trả về chuỗi biểu diễn cho mesh
    }
}
