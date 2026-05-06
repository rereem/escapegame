using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class Drawer_Pull_X : MonoBehaviour, IInteractable
    {
        public Animator pull_01;
        public bool open;
        public Transform Player;
        public GameObject promptCanvas;

        void Start()
        {
            open = false;
            if (promptCanvas != null) promptCanvas.SetActive(false);
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
            pull_01.Play("openpull_01");
            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            pull_01.Play("closepush_01");
            open = false;
            yield return new WaitForSeconds(.5f);
        }
    }
}