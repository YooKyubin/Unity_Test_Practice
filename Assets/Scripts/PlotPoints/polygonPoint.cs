using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class polygonPoint : MonoBehaviour
{
    public float size = 1.0f;
    public int numPoints = 3;
    public float turnFraction = 0.98f;
    public float angle = 180f;
    Mesh mesh;
    Vector3[] vertices;
    int[] indices;
    float timeElips;

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
        timeElips = 0;
    }

    void Update()
    {
        timeElips += Time.deltaTime;
        float t = timeElips * (2 * Mathf.PI) / 4;
        numPoints = (int)((Mathf.Sin(t)/ 2 + 0.5f) * 150);

        SetMeshData(size, numPoints);
        CreateProceduralMeshPoint();
        
    }

    void SetMeshData(float size, int numPoints)
    {
        vertices = new Vector3[numPoints];

        float thetaRange = Mathf.Cos(angle * Mathf.Deg2Rad);
        for (int i = 0; i < numPoints; ++i)
        {
            float t = numPoints == 1 ? 0 : i / (numPoints - 1f);

            float inclination = Mathf.Acos(1 - (1 - thetaRange) * t); // 1 - 2*t는 arccos 정의역(1 ~ -1), inclination => (0 ~ pi), 감소함수
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
        // mesh.triangles = indices; // mesh.triagles의 배열 길이는 vertices.Length * 3 이어야 한다.
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }
}
