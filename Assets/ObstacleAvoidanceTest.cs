using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleAvoidanceTest : MonoBehaviour
{
    public float distance;
    public float angle;
    public float speed;
    public float rayRadius;
    public float turnSpeed;

    private const int numPoints = 30;
    [SerializeField]
    LayerMask obstacleLayer;
    Transform detectPos;
    Vector3[] dirs;
    // Start is called before the first frame update
    void Start()
    {
        float total = 
        speed = Random.Range(1.5f, 3.0f);
        distance = Random.Range(3.0f, 7.0f);
        rayRadius = Random.Range(0.4f, 1.0f);
        turnSpeed = Random.Range(1f, 10f);

        dirs = new Vector3[numPoints];

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
            Vector3 dir = transform.localRotation * dirs[i]; // dir을 로컬 좌표계로 변환하기 위함
            //Vector3 dir = transform.TransformDirection(dirs[i]);

            if (Physics.SphereCast(detectPos.position, rayRadius, dir, out hit, this.distance, obstacleLayer))
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
        //Debug.Log(dirs[2]); // 매 update 마다 localRoation 적용 이전의 dir 을 구할 필요가 없음

        moveDir = Vector3.Lerp(this.transform.forward, moveDir, Time.deltaTime * turnSpeed);
        moveDir.Normalize();

        MoveFoward(moveDir);
        
        // 속도, 광선 길이, sphere cast의 반지름, lerp 속도 등으로 다양하게 회전 구현 가능
    }


    void MoveFoward(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
