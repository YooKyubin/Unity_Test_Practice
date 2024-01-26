using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZValue : MonoBehaviour
{
    public float zValue;
    public static ChangeZValue Instance;

    void Awake()
    {
        Instance = this;
    }
}