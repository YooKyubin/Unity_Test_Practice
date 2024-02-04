using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygonPoint : MonoBehaviour
{
    public float size = 1.0f;
    public int numPoints = 3;
    public float turnFraction;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    private void OnValidate()
    {
        if (mesh == null)
            return;

        if (size > 0 && numPoints >= 3)
        {
            SetMeshData(size, numPoints);
            CreateProceduralMeshPoint();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        SetMeshData(size, numPoints);
        CreateProceduralMeshPoint();
    }


    void SetMeshData(float size, int numPoints)
    {
        vertices = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1.0f);
            float inclination = Mathf.Acos(1 - 2 * t); // 1 - 2*t는 arccos 정의역(1 ~ -1), inclination => (0 ~ pi), 감소함수
            float azimuth = 2 * Mathf.PI * i * turnFraction; // 방위각

            float x = size * Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = size * Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = size * Mathf.Cos(inclination);

            //Vector3 pos = transform.TransformPoint(new Vector3(x, y, z));
            Vector3 pos = new Vector3(x, y, z);

            vertices[i] = pos;
        }

        triangles = new int[3 * numPoints];
        for (int i = 0; i < numPoints - 2; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        triangles[3 * numPoints - 3] = 0;
        triangles[3 * numPoints - 2] = numPoints-1;
        triangles[3 * numPoints - 1] = 1;
    }

    void CreateProceduralMeshPoint()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.SetIndices(mesh.GetIndices(0), MeshTopology.Points, 0);
    }
}
