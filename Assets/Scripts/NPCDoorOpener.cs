using UnityEngine;
using SojaExiles;

public class NPCDoorOpener : MonoBehaviour
{
    float checkRadius = 2f;
    float cooldown = 0f;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0f) return;

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject doorObj in doors)
        {
            float dist = Vector3.Distance(transform.position, doorObj.transform.position);
            Debug.Log("Door distance: " + dist + " checkRadius: " + checkRadius);
            if (dist < checkRadius)
            {
                opencloseDoor door = doorObj.GetComponentInChildren<opencloseDoor>();
                if (door == null) door = doorObj.GetComponent<opencloseDoor>();
                Debug.Log("Door script found: " + door);
                if (door != null && !door.open)
                {
                    door.OpenDoor();
                    cooldown = 2f;
                }
            }
        }
    }
}