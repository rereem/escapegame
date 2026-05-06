using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor1 : MonoBehaviour, IInteractable
    {
        public Animator openandclose1;
        public bool open;
        public Transform Player;
        public GameObject promptCanvas;

        void Start()
        {
            open = false;
            if (promptCanvas == null) return;

            promptCanvas.SetActive(false);
        }

        public void ShowPrompt(bool show)
        {
            if (promptCanvas == null) return;
            promptCanvas.SetActive(show);
        }

        public void Interact()
        {
            if (open == false)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        IEnumerator opening()
        {
            openandclose1.Play("Opening 1");
            open = true;
            yield return new WaitForSeconds(.5f);
            GetComponent<Collider>().enabled = false;
        }

        IEnumerator closing()
        {
            openandclose1.Play("Closing 1");
            open = false;
            yield return new WaitForSeconds(.5f);
            GetComponent<Collider>().enabled = true;
        }
    }
}