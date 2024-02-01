using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleAvoidanceTest : MonoBehaviour
{
    public float distance;
    private const int numPoints = 30;
    public float angle;
    [SerializeField]
    LayerMask obstacleLayer;
    Transform detectPos;
    Vector3[] dirs;
    // Start is called before the first frame update
    void Start()
    {
        dirs = new Vector3[numPoints];
        distance = 2.0f;
        //numPoints = 30;
        angle = Mathf.PI * 0.5f;
        obstacleLayer = LayerMask.GetMask("Obstacle");
        this.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
        detectPos = this.transform.GetChild(0).transform;

        for (int i=0; i<numPoints; i++)
        {
            float theta = angle * i / (numPoints - 1f);
            float z = Mathf.Cos(theta);
            float x = Mathf.Sin(theta) * Mathf.Pow(-1, i % 2);
            Vector3 dir = new Vector3(x, 0, z);
            dirs[i] = dir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<numPoints; i++)
        {
            //float theta = angle * i / (numPoints - 1f);
            //float z = Mathf.Cos(theta);
            //float x = Mathf.Sin(theta) * Mathf.Pow(-1, i % 2);
            //Vector3 dir = new Vector3(x, 0, z);
            //dirs[i] = dir;
            // -------------- �� ���� ������� start �� �ѹ������ε� �����غ��� ----------
            Color color = Color.green;
            Vector3 dir = transform.localRotation * dirs[i]; // dir�� ���� ��ǥ��� ��ȯ�ϱ� ����
            //dirs[i] = dir;

            if (Physics.Raycast(detectPos.position, dir, this.distance, obstacleLayer))
            {
                color = Color.red;
            }

            Debug.DrawLine(detectPos.position, detectPos.position + dir * distance, color);
        }
        Debug.Log(dirs[2]); // �� update ���� localRoation ���� ������ dir �� ���� �ʿ䰡 ����
    }
}
