using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceTest3D : MonoBehaviour
{
    public float distance;
    public float angle;
    public float speed;
    public float rayRadius;
    public float turnSpeed;

    private const int numPoints = 300;
    [SerializeField]
    LayerMask obstacleLayer;
    Transform detectTransform;
    Vector3[] dirs;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2.5f, 5.0f);
        distance = Random.Range(3.0f, 7.0f);
        rayRadius = Random.Range(0.4f, 1.0f);
        turnSpeed = Random.Range(15f, 25f);

        dirs = new Vector3[numPoints];

        angle = 135f;

        obstacleLayer = LayerMask.GetMask("Obstacle");
        this.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(Random.value, Random.value, Random.value);
        detectTransform = this.transform.GetChild(0).transform;

        float thetaRange = Mathf.Cos(Mathf.Deg2Rad * angle);
        for (int i=0; i<numPoints; i++)
        {
            float t = i / (numPoints - 1.0f);
            float inclination = Mathf.Acos(1 - (1 - thetaRange) * t); // 1 - 2*t는 arccos 정의역(1 ~ -1), inclination => (0 ~ pi), 감소함수
            float azimuth = 2 * Mathf.PI * i * 1.618f; // 방위각

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            dirs[i] = new Vector3(x, y, z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 detectPos = detectTransform.position;
        Vector3 moveDir = this.transform.forward;
        RaycastHit hit;
        float maxDist = 0f;
        for (int i = 0; i < numPoints; i++)
        {
            UnityEngine.Color color = UnityEngine.Color.green;
            // Vector3 dir = transform.localRotation * dirs[i]; // dir을 로컬 좌표계로 변환하기 위함
            Vector3 dir = transform.TransformDirection(dirs[i]);

            if (Physics.SphereCast(detectPos, rayRadius, dir, out hit, this.distance, obstacleLayer))
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

            Debug.DrawLine(detectPos, detectPos + dir * distance, color);

        }

        Debug.DrawLine(detectPos, detectPos + moveDir * distance, UnityEngine.Color.green);

        moveDir = Vector3.Lerp(this.transform.forward, moveDir, Time.deltaTime * turnSpeed);
        moveDir.Normalize();

        MoveFoward(moveDir);
    }

    void MoveFoward(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
