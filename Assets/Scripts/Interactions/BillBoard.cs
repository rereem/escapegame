using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BillBoard : MonoBehaviour
{
    void LateUpdate()
    {
        var cam = Camera.main;
        if (!cam) return;

        Vector3 camEuler = cam.transform.rotation.eulerAngles;
        Vector3 myEuler = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(myEuler.x, camEuler.y , camEuler.z);
    }
}
