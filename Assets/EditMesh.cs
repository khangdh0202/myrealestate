using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMesh : MonoBehaviour
{
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        /*mesh.Clear();

        // Do some calculations...
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;*/
    }
}
