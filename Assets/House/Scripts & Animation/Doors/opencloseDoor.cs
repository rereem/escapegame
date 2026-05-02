using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;

        void Start()
        {
            open = false;
            GetComponent<Collider>().enabled = true;
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
    }
}