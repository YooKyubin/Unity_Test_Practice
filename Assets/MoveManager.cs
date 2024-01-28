using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public GameObject prefab;
    public float seperationWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float neighborRange = 20.0f;
    public float speed = 1.0f;

    const int number = 100;
    GameObject[] objects = new GameObject[number];


    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<number; ++i)
        {
            GameObject parent = new GameObject();
            parent.name = "obj".Insert( 3, i.ToString() );
            parent.transform.parent = this.transform;
            objects[i] = parent; 

            GameObject temp = MonoBehaviour.Instantiate(prefab);
            temp.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.forward);

            temp.transform.parent = parent.transform;

            objects[i].transform.SetPositionAndRotation(Random.insideUnitSphere * 10 + this.transform.position, Random.rotationUniform );
            // * temp.transform.rotation;

            //objects[i].transform.localPosition = Random.insideUnitSphere;
            //objects[i].transform.localRotation = Random.rotationUniform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject cur = objects[i];

            // cur.transform.position += cur.transform.localRotation * Vector3.forward * Time.deltaTime;
            //Quaternion * Vector3 연산은 Vector3 벡터를 Quaternion 회전시킨 벡터
            List<int> neighbors = new List<int>();
            List<int> seperationNeighbor = new List<int>();

            for (int j = 0; j < number; ++j)
            {
                if (i == j)
                    continue;

                if ((cur.transform.position - objects[j].transform.position).magnitude <= neighborRange)
                    neighbors.Add(j);

                if ((cur.transform.position - objects[j].transform.position).magnitude <= neighborRange / 5)
                    seperationNeighbor.Add(j);

            }

            Vector3 direction = Vector3.zero;
            direction += GetSeperation(i, seperationNeighbor);
            direction += GetCohesion(i, neighbors);
            direction += GetAlignment(i, neighbors);



            direction = Vector3.Lerp(cur.transform.forward, direction, Time.deltaTime);
            direction.Normalize();
            Vector3 curPos = cur.transform.localPosition;
            if (cur.transform.localPosition.y < -14 || cur.transform.localPosition.y >= 14)
                curPos.y = -curPos.y;

            if (cur.transform.position.x < -24 || cur.transform.position.x >= 24)
                curPos.x = -curPos.x;

            if (cur.transform.position.z < -24 || cur.transform.position.z >= 24)
                curPos.z = -curPos.z;
            
            cur.transform.localPosition = curPos;
            //if (cur.transform.position.y < 0 || cur.transform.position.y >= 30)
            //    direction.y = -direction.y;

            //if (cur.transform.position.x < -24 || cur.transform.position.x >= 24)
            //    direction.x = -direction.x;

            //if (cur.transform.position.z < -24 || cur.transform.position.z >= 24)
            //    direction.z = -direction.z;

            cur.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
            cur.transform.position += cur.transform.forward * Time.deltaTime * speed;
        }
    }
    Vector3 GetSeperation(int cur, List<int> around)
    {
        if (around == null || around.Count == 0)
            return Vector3.zero;

        Vector3 vec = Vector3.zero;
        for (int i=0; i<around.Count; ++i)
        {
            vec += objects[cur].transform.position - objects[i].transform.position;
        }

        vec.Normalize();
        vec += Random.onUnitSphere;
        return vec.normalized * seperationWeight * 4;
    }

    Vector3 GetCohesion(int cur, List<int> around)
    {
        if (around == null || around.Count == 0)
            return Vector3.zero;

        Vector3 vec = Vector3.zero;
        for (int i = 0; i < around.Count; ++i)
        {
            vec += objects[i].transform.position;
        }
        vec /= around.Count;
        vec -= objects[cur].transform.position;
            
        return vec.normalized * cohesionWeight;
    }

    Vector3 GetAlignment(int cur, List<int> around)
    {
        if (around == null || around.Count == 0)
            return objects[cur].transform.forward;

        Vector3 vec = Vector3.zero;
        for (int i = 0; i < around.Count; ++i)
        {
            vec += objects[i].transform.forward;
        }
        vec /= around.Count;

        return vec.normalized * alignmentWeight;
    }

}

