using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    public Material Red;
    public Material Blue;


    // Start is called before the first frame update
    void Start()
    {
        for (float i= -this.transform.localScale.x * 5 + 1; i < this.transform.localScale.x * 5; i += 2)
        {
            for (float j= -this.transform.localScale.z * 5 + 1; j< this.transform.localScale.z * 5; j += 2)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i, 0.5f, j);

                if(Random.Range(0, 50) > 10f)
                {
                    cube.GetComponent<MeshRenderer>().material = Red;
                    cube.layer = LayerMask.NameToLayer("Red");
                }
                else
                {
                    cube.GetComponent<MeshRenderer>().material = Blue;
                    cube.layer = LayerMask.NameToLayer("Blue");
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
