using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleAvoidanceTest : MonoBehaviour
{
    public float distance;
    private const int numPoints = 30;
    public float angle;
    public float speed;
    [SerializeField]
    LayerMask obstacleLayer;
    Transform detectPos;
    Vector3[] dirs;
    // Start is called before the first frame update
    void Start()
    {
        dirs = new Vector3[numPoints];
        distance = 7.0f;
        //numPoints = 30;
        angle = Mathf.PI * 0.75f;

        obstacleLayer = LayerMask.GetMask("Obstacle");
        this.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(Random.value, Random.value, Random.value);
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
        Vector3 moveDir = this.transform.forward;
        RaycastHit hit;
        float maxDist = 0f;
        for (int i=0; i<numPoints; i++)
        {
            UnityEngine.Color color = UnityEngine.Color.green;
            Vector3 dir = transform.localRotation * dirs[i]; // dir�� ���� ��ǥ��� ��ȯ�ϱ� ����
            //Vector3 dir = transform.TransformDirection(dirs[i]);

            if (Physics.SphereCast(detectPos.position, 0.5f, dir, out hit, this.distance, obstacleLayer))
            {
                color = UnityEngine.Color.red;
                if (hit.distance > maxDist)
                {
                    maxDist = hit.distance;
                    moveDir = dir;
                }
            }
            else
            {
                moveDir = dir;
                break;
            }


            Debug.DrawLine(detectPos.position, detectPos.position + dir * distance, color);
        }

        Debug.DrawLine(detectPos.position, detectPos.position + moveDir * distance, UnityEngine.Color.green);
        //Debug.Log(dirs[2]); // �� update ���� localRoation ���� ������ dir �� ���� �ʿ䰡 ����

        moveDir = Vector3.Lerp(this.transform.forward, moveDir, Time.deltaTime * 5);
        moveDir.Normalize();

        MoveFoward(moveDir);
        
        // �ӵ�, ���� ����, sphere cast�� ������, lerp �ӵ� ������ �پ��ϰ� ȸ�� ���� ����
    }


    void MoveFoward(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}