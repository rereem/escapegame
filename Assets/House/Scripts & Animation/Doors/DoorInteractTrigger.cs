using SojaExiles;
using UnityEngine;

public class DoorInteractTrigger : MonoBehaviour
{
    opencloseDoor door;
    bool playerNearby = false;
    bool isAnimating = false;

    void Start()
    {
        door = GetComponentInParent<opencloseDoor>();
        Debug.Log("Door found: " + door);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    void Update()
    {
        if (door == null) return; // add this

        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            isAnimating = true;
            if (door.open)
                door.CloseDoor();
            else
                door.OpenDoor();
            Invoke("ResetAnimating", 1f);
        }
    }

    void ResetAnimating()
    {
        isAnimating = false;
    }
}