using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class OvenFlip : MonoBehaviour, IInteractable
    {
        public Animator openandcloseoven;
        public bool open;
        public Transform Player;
        public static bool isUnlocked = false;
        public GameObject promptCanvas;

        void Start()
        {
            open = false;
            if (promptCanvas != null) promptCanvas.SetActive(false);
        }

        public void ShowPrompt(bool show)
        {
            if (promptCanvas == null) return;
            // only show prompt if oven is unlocked!
            promptCanvas.SetActive(show && isUnlocked);
        }

        public void Interact()
        {
            if (!isUnlocked) return;
            if (Time.timeScale == 0f) return;

            if (!open)
                StartCoroutine(opening());
            else
                StartCoroutine(closing());
        }

        public void OpenOven()
        {
            if (!open)
                StartCoroutine(opening());
        }

        IEnumerator opening()
        {
            openandcloseoven.Play("OpenOven");
            open = true;
            yield return new WaitForSeconds(.5f);
            Keypickup key = FindObjectOfType<Keypickup>();
            if (key != null) key.EnableCollider();
        }

        IEnumerator closing()
        {
            openandcloseoven.Play("ClosingOven");
            open = false;
            yield return new WaitForSeconds(.5f);
            Keypickup key = FindObjectOfType<Keypickup>();
            if (key != null) key.DisableCollider();
        }
    }
}