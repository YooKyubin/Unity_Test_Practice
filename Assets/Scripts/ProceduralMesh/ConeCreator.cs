using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConeCreator : MonoBehaviour
{
    public float radius = 1.0f;
    public float height = 1.0f;
    public int polygon = 20;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    private void OnValidate()
    {
        if (mesh == null)
            return;

        if (radius > 0 && polygon >= 3 && height > 0)
        {
            SetMeshData(radius, polygon);
            CreateProceduralMesh();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        polygon = 20;

        SetMeshData(radius, polygon);
        CreateProceduralMesh();
    }


    void SetMeshData(float radius, int polygon)
    {
        vertices = new Vector3[(polygon + 1) * 2];
        vertices[0] = new Vector3(0, -height / 2.0f, 0);
        for (int i = 1; i <= polygon; i++)
        {
            float angle = Mathf.PI * 2.0f * -i / polygon;

            vertices[i] = new Vector3(Mathf.Cos(angle) * radius, -height / 2.0f, Mathf.Sin(angle) * radius);
        }

        triangles = new int[3 * polygon * 2];
        for (int i = 0; i < polygon - 1; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        triangles[3 * polygon - 3] = 0;
        triangles[3 * polygon - 2] = 1;
        triangles[3 * polygon - 1] = polygon;



        int vIdx = polygon + 1;
        vertices[vIdx] = new Vector3(0, height / 2.0f, 0);
        ++vIdx;
        for (int i = 1; i <= polygon; i++)
        {
            float angle = Mathf.PI * 2.0f * -i / polygon;

            vertices[vIdx] = new Vector3(Mathf.Cos(angle) * radius, -height / 2.0f, Mathf.Sin(angle) * radius);
            ++vIdx;
        }

        int tIdx = 3 * polygon;
        for (int i = 0; i < polygon - 1; i++)
        {
            triangles[tIdx++] = (polygon + 1) + i + 1;
            triangles[tIdx++] = (polygon + 1) + i + 2;
            triangles[tIdx++] = (polygon + 1);
        }

        triangles[tIdx++] = (polygon + 1) + polygon;
        triangles[tIdx++] = (polygon + 1) + 1;
        triangles[tIdx++] = (polygon + 1);
    }

    void CreateProceduralMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
