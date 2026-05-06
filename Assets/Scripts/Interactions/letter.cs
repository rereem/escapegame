using UnityEngine;
using TMPro;
using StarterAssets;

public class letter : MonoBehaviour, IInteractable
{
    public GameObject letterUI;
    public FirstPersonController player;
    public Renderer letterMesh;
    public GameObject promptCanvas;
    public TextMeshProUGUI code;
    public TextMeshProUGUI codeUI;


    bool toggle;
    void Start()
    {
        code.text = CodeManager.secretCode.ToString();
        codeUI.text = CodeManager.secretCode.ToString();
    }

    // TRIGGER handles showing prompt
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player") &&
    //        !other.transform.root.CompareTag("Player")) return;
    //    promptCanvas.SetActive(true);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Player") &&
    //        !other.transform.root.CompareTag("Player")) return;
    //    promptCanvas.SetActive(false);
    //}

    // INTERFACE still needed but prompt handled by trigger now
    public void ShowPrompt(bool show)
    {
        promptCanvas.SetActive(show); // stop leaving this empty!
    }

    public void Interact()
    {
        toggle = !toggle;
        promptCanvas.SetActive(false);
        NotificationManager.Show("📄 You found a note!");

        if (toggle)
        {
            letterUI.SetActive(true);
            letterMesh.enabled = false;
            player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            letterUI.SetActive(false);
            letterMesh.enabled = true;
            player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}