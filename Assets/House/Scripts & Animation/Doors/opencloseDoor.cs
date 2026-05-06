using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour, IInteractable
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public GameObject promptCanvas;
        public GameObject promptCanvas2;

        public bool requiresKey = false;

        void Start()
        {
            open = false;
            GetComponent<Collider>().enabled = true;
            if (promptCanvas == null) return;
            if (promptCanvas2 == null) return;
            promptCanvas.SetActive(false);
            promptCanvas2.SetActive(false);
        }

        public void ShowPrompt(bool show)
        {
            if (promptCanvas == null) return;
            if (promptCanvas2 == null) return;
            promptCanvas.SetActive(show);
            promptCanvas2.SetActive(show);
        }

        IEnumerator opening()
        {
            print("you are opening the door");
            openandclose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(.5f);
            GetComponent<Collider>().enabled = false;
        }

        IEnumerator closing()
        {
            print("you are closing the door");
            openandclose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(.5f);
            GetComponent<Collider>().enabled = true;
        }

        public void OpenDoor()
        {
            if (!open)
                StartCoroutine(opening());
        }

        public void CloseDoor()
        {
            if (open)
                StartCoroutine(closing());
        }

        public void Interact()
        {
            // Check if this specific door needs a key
            if (requiresKey && !GameManager.instance.hasKey)
            {
                Debug.Log("Door locked - no key");
                NotificationManager.Show(" You need a key!");
                return;
            }

            // Normal behavior
            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }
    }
}