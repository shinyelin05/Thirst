using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    public void Awake()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}
