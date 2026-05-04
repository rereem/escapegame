using UnityEngine;
using TMPro;
using SojaExiles;
using System.Collections;

public class KeypadManager : MonoBehaviour
{
    [Header("Keypad UI")]
    public GameObject keypadUI;

    [Header("4 Input Boxes")]
    public TextMeshProUGUI[] inputBoxes;

    [Header("References")]
    public GameObject keyObject;
    public GameObject Padlock;
    public Animator padlockAnimator;
    public float interactRange = 2f;
    public Transform player;
    public Animator keypadAnimator;

    string currentInput = "";
    bool keypadOpen = false;
    bool codeEntered = false;


    void Start()
    {
        keypadUI.SetActive(false);
        //if (keyObject != null)
        //    keyObject.SetActive(false);
        ClearBoxes();
    }

    void Update()
    {
        if (codeEntered) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < interactRange && Input.GetKeyDown(KeyCode.E))
        {
            keypadOpen = !keypadOpen;
            keypadUI.SetActive(keypadOpen);
            currentInput = "";
            ClearBoxes();

            if (keypadOpen)
            {
                // show cursor + pause game
                keypadAnimator.SetTrigger("Open");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
            else
            {
                // hide cursor + unpause game
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }

        // close if player walks away
        if (dist >= interactRange && keypadOpen)
        {
            keypadOpen = false;
            keypadUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    // Called from each number button
    public void PressNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            UpdateBoxes();
        }
    }
    // Called from Close button
    public void CloseKeypad()
    {
        keypadOpen = false;
        keypadUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    // Called from Ok button
    public void ConfirmCode()
    {
        if (currentInput.Length < 4)
        {
            StartCoroutine(ShowWrong());
            return;
        }

        if (currentInput == CodeManager.secretCode)
        {
            Debug.Log("Correct!");
            codeEntered = true;
            keypadUI.SetActive(false);
            keypadOpen = false;

            // hide cursor + unpause
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            // play padlock opening animation
            if (padlockAnimator != null)
                padlockAnimator.SetTrigger("Open");

            // spawn the key
            //if (keyObject != null)
            //    keyObject.SetActive(true);

            OvenFlip.isUnlocked = true; // unlock the oven
         
            DisableWithDelay();


        }
        else
        {
            Debug.Log("Wrong!");
            StartCoroutine(ShowWrong());
        }
    }

    // Called from Delete button
    public void ClearInput()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateBoxes();
        }
    }

    void UpdateBoxes()
    {
        ClearBoxes();
        for (int i = 0; i < currentInput.Length; i++)
        {
            inputBoxes[i].text = currentInput[i].ToString();
        }
    }

    void ClearBoxes()
    {
        foreach (TextMeshProUGUI box in inputBoxes)
        {
            box.text = "";
        }
    }

    System.Collections.IEnumerator ShowWrong()
    {
        foreach (TextMeshProUGUI box in inputBoxes)
            box.text = "X";
        // use WaitForSecondsRealtime because Time.timeScale is 0
        yield return new WaitForSecondsRealtime(0.8f);
        currentInput = "";
        ClearBoxes();
    }
    public void DisableWithDelay()
    {
        StartCoroutine(DisableAfterDelay());
    }

    IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Padlock.SetActive(false);
    }
  
}