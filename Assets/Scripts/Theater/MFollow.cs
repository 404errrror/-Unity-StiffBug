using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFollow : MonoBehaviour
{
    public GameObject following;
    public Vector3 offset;

    void Update()
    {
        transform.position = following.transform.position + offset;
    }
}
