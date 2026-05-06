using UnityEngine;
using SojaExiles;
using System.Collections;

public class ExitDoorTrigger : MonoBehaviour
{
    public opencloseDoor door; // drag the exit door here in Inspector
    private bool playerNearby = false;

    private void Update()
    {
        if (!playerNearby) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed, hasKey: " + GameManager.instance.hasKey);

            if (GameManager.instance.hasKey)
            {
                Debug.Log("Opening door!");
                door.Interact();
            }
            else
            {
                Debug.Log("Need a key!");
                NotificationManager.Show(" You need a key to escape!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
    IEnumerator WinAfterDelay()
    {
        yield return new WaitForSeconds(1f); // wait for door to open
        //GameManager.instance.Win();
    }
}