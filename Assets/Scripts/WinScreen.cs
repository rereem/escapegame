using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered: " + other.name + " tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("You Won!");
            //GameManager.instance.Win(); // uncomment when ready
        }
    }
}

