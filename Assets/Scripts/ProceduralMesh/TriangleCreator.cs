using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TriangleCreator : MonoBehaviour
{
    public float size = 1.0f;
    public bool vertex6 = false;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    delegate void SetMeshDataDelegate();
    SetMeshDataDelegate setMeshDataDelegate;

    private void OnValidate()
    {
        if (mesh == null)
            return;

        if (vertex6)
        {
            setMeshDataDelegate = SetMeshData6;
        }
        else
        {
            setMeshDataDelegate = SetMeshData;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        setMeshDataDelegate = SetMeshData;
    }

    void Update()
    {

        setMeshDataDelegate();
        CreateProceduralMesh();
    }


    void SetMeshData()
    {
        vertices = new Vector3[] {
            new Vector3(-0.5f, 0.5f, ChangeZValue.Instance.zValue),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
        };

        for (int i=0; i<vertices.Length; i++)
        {
            vertices[i] *= size;
        }

        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void SetMeshData6()
    {
        vertices = new Vector3[] {
            new Vector3(-0.5f, 0.5f, ChangeZValue.Instance.zValue),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0),

            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
        };

        for (int i=0; i<vertices.Length; i++)
        {
            vertices[i] *= size;
        }

        triangles = new int[] { 0, 1, 2, 3, 4, 5 };
        
    }

    void CreateProceduralMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


    }
}
