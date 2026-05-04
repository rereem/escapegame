using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class OvenFlip : MonoBehaviour
    {
        public Animator openandcloseoven;
        public bool open;
        public Transform Player;
        public static bool isUnlocked = false;

        void Start()
        {
            open = false;
        }

        void OnMouseOver()
        {
            if (Player)
            {
                // don't interact if keypad is open // or if game is paused
                if (Time.timeScale == 0f) return;
                if (!isUnlocked)  return;

                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 15)
                {
                    if (open == false)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            StartCoroutine(opening());

                        }
                    }
                    else
                    {
                        if (open == true)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                StartCoroutine(closing());
                            }
                        }
                    }
                }
            }
        }

        // Called by KeypadManager when correct code is entered
        public void OpenOven()
        {
            if (!open)
                StartCoroutine(opening());
        }

        IEnumerator opening()
        {
            print("you are opening the oven");
            openandcloseoven.Play("OpenOven");
            open = true;
            yield return new WaitForSeconds(.5f);

            Keypickup key = FindObjectOfType<Keypickup>();
            if (key != null)
                key.EnableCollider();

        }

        IEnumerator closing()
        {
            print("you are closing the oven");
            openandcloseoven.Play("ClosingOven");
            open = false;
            yield return new WaitForSeconds(.5f);

            Keypickup key = FindObjectOfType<Keypickup>();
            if (key != null)
                key.DisableCollider();
        }
       
    }
}