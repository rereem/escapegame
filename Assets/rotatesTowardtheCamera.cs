using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatesTowardtheCamera : MonoBehaviour
{
    private Camera mianCamera;
    void Start()
    {
        mianCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mianCamera != null)
        {
            transform.LookAt(transform.position + mianCamera.transform.rotation * Vector3.forward, mianCamera.transform.rotation * Vector3.up);
        }
    }
}
