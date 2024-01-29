using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float distance = 1.0f;

    [SerializeField] private Collider[] colls;

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
        colls = Physics.OverlapSphere(this.transform.position, distance, layerMask);

        for (int i=0; i<colls.Length; i++)
        {
            Collider c = colls[i];
            Color black = Color.black;
            Debug.DrawLine(this.transform.position, c.transform.position, black, 0, false);
        }


        Range.transform.localScale = Vector3.one * distance * 2;
    }
}
