using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCreator : MonoBehaviour
{
    public int numPoints;
    public float turnFraction = 1.618f;
    public Mesh Mesh;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        //for (int i=0; i<numPoints; i++)
        //{
        //    float dst = Mathf.Pow(2 * i / (numPoints - 1.0f), 0.5f);
        //    float angle = 2 * Mathf.PI * i * turnFraction;

        //    float x = dst * Mathf.Cos(angle);
        //    float y = dst * Mathf.Sin(angle);

        //    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    Vector3 pos = transform.TransformPoint(new Vector3(x, y, 0));
        //    temp.transform.position = pos;
        //    temp.transform.localScale = Vector3.one * 0.05f;

        //    Vector3 c = Vector3.one * (i / (numPoints - 1.0f));
        //    temp.GetComponent<Renderer>().material.color = new Color(c.x, c.y, c.x);
        //    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    Vector3 pos = transform.TransformPoint(new Vector3(x, y, z));
        //    temp.transform.position = pos;
        //    temp.transform.localScale = Vector3.one * 0.05f;

        //    Vector3 c = Vector3.one * (i / (numPoints - 1.0f));
        //    temp.GetComponent<Renderer>().material.color = new Color(c.x, c.y, c.x);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        float thetaRange = Mathf.Cos(Mathf.Deg2Rad * angle);

        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1.0f);
            
            float inclination = Mathf.Acos(1 - (1 - thetaRange) * t); // 1 - 2*t는 arccos 정의역(1 ~ -1), inclination => (0 ~ pi), 감소함수
            float azimuth = 2 * Mathf.PI * i * turnFraction; // 방위각
            //float azimuth = t * Mathf.PI * 2;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            Vector3 pos = transform.TransformPoint(new Vector3(x, y, z));

            Vector3 c = Vector3.one * (1 - i / (numPoints - 1.0f));

            Debug.DrawLine(this.transform.position, pos, new Color(c.x, c.y, c.z));
        }

    }
}
