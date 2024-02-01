using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float distance = 1.0f;

    public Collider[] colls;
    public Collider[] preColls;

    LayerMask layerMask;
    GameObject Range;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Red");
        Range = this.transform.Find("Range").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // OverlapSphereNonAlloc 사용이 가비지컬렉터를 발생시키지 않아 권장된다.
        colls = Physics.OverlapSphere(this.transform.position, distance, layerMask);

        for (int i=0; i<colls.Length; i++)
        {
            Collider c = colls[i];
            Color black = Color.black;
            Debug.DrawLine(this.transform.position, c.transform.position, black, 0, false);
        }

        if (!Enumerable.SequenceEqual(colls, preColls))
        {
            for (int i = 0; i < preColls.Length; i++)
            {
                preColls[i].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            }


            for (int i = 0; i < colls.Length; i++)
            {
                colls[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        preColls = colls;


        Range.transform.localScale = Vector3.one * distance * 2;
    }
}
