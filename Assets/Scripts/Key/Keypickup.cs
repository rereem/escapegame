using UnityEngine;
using SojaExiles;
using System.Collections;

public class Keypickup : MonoBehaviour
{
    //public GameObject promptUI; // "Press E to Pick Up" popup

    private bool playerNearby = false;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        

        if (!playerNearby) return;
        if (!OvenFlip.isUnlocked) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.hasKey = true;
            Debug.Log("Key picked up!");
            //promptUI.SetActive(false);
            gameObject.SetActive(false); // key disappears
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit by: " + other.name + " tag: " + other.tag);
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player")) return;
        playerNearby = true;
       // if (OvenFlip.isUnlocked)
        //promptUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = false;
        //promptUI.SetActive(false);
    }

    public void EnableCollider()
    {
        StartCoroutine(EnableAfterDelay());
    }
    public void DisableCollider() 
    {
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(0.8f); // small delay
        GetComponent<Collider>().enabled = true;
    }
}