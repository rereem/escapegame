using UnityEngine;
using SojaExiles;
using System.Collections;

public class Keypickup : MonoBehaviour, IInteractable
{
    public GameObject promptCanvas;
    private bool playerNearby = false;
    public Collider raycastCollider;  // always enabled, for raycast
    public Collider triggerCollider;  // enables when oven opens, for pickup

    private void Start()
    {
        triggerCollider.enabled = false;
       // GetComponent<Collider>().enabled = false;
        if (promptCanvas != null) promptCanvas.SetActive(false);

    }

    public void ShowPrompt(bool show)
    {
        if (promptCanvas == null) return;
        // only show if oven is actually open!
        OvenFlip oven = FindObjectOfType<OvenFlip>();
        bool ovenOpen = oven != null && oven.open;
        promptCanvas.SetActive(show && OvenFlip.isUnlocked && ovenOpen);
    }

    public void Interact()
    {
        if (!OvenFlip.isUnlocked) return;
        GameManager.instance.hasKey = true;
        Debug.Log("Key picked up!");
        if (promptCanvas != null) promptCanvas.SetActive(false);
        gameObject.SetActive(false);
        NotificationManager.Show(" Key picked up!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player")) return;
        playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = false;
    }

    //public void EnableCollider()
    //{
    //    StartCoroutine(EnableAfterDelay());
    //}

    //public void DisableCollider()
    //{
    //    GetComponent<Collider>().enabled = false;
    //}

    //IEnumerator EnableAfterDelay()
    //{
    //    yield return new WaitForSeconds(0.8f);
    //    GetComponent<Collider>().enabled = true;
    //}

    public void EnableCollider()
    {
        StartCoroutine(EnableAfterDelay());
    }

    IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(0.8f);
        triggerCollider.enabled = true;
    }

    public void DisableCollider()
    {
        triggerCollider.enabled = false;
    }
}