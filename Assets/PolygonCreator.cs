using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class PolygonCreator : MonoBehaviour
{
    public float size = 1.0f;
    public int polygon = 3;

    Mesh mesh;
    MeshCollider meshCollider;
    Vector3[] vertices;
    int[] triangles;

    private void OnValidate()
    {
        if (mesh == null)
            return;

        if (size > 0 && polygon >= 3)
        {
            SetMeshData(size, polygon);
            CreateProceduralMesh();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();

        SetMeshData(size, polygon);
        CreateProceduralMesh();
    }


    void SetMeshData(float size, int polygon)
    {
        vertices = new Vector3[polygon + 1];
        vertices[0] = new Vector3(0, 0, 0);
        for (int i=1; i<=polygon; i++) 
        {
            float angle = Mathf.PI * 2.0f * -i / polygon;

            vertices[i] = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * size;
        }

        triangles = new int[3 * polygon];
        for (int i=0; i<polygon - 1; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        triangles[3 * polygon - 3] = 0;
        triangles[3 * polygon - 2] = polygon;
        triangles[3 * polygon - 1] = 1;
    }

    void CreateProceduralMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }
}
