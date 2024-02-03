using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plotter : MonoBehaviour
{
    MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Points, 0);
    }

    // Update is called once per frame1
    void Update()
    {
        
    }
}
