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
    int[] indices;

    private void OnValidate()
    {
        if (mesh == null)
            return;

        if (size > 0 && numPoints >= 1)
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

        for (int i=0; i<numPoints; ++i)
        {
            float t;

            if (numPoints != 1)
                t = i / (numPoints - 1.0f);
            else
                t = 0;

            float inclination = Mathf.Acos(1 - 2 * t); // 1 - 2*t는 arccos 정의역(1 ~ -1), inclination => (0 ~ pi), 감소함수
            float azimuth = 2 * Mathf.PI * i * turnFraction; // 방위각

            float x = size * Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = size * Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = size * Mathf.Cos(inclination);

            vertices[i] = new Vector3(x, y, z);
        }

        indices = new int[numPoints];
        for (int i = 0; i < numPoints; ++i)
        {
            indices[i] = i;
        }
    }

    void CreateProceduralMeshPoint()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }
}
