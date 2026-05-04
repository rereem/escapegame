using UnityEngine;
using TMPro;

public class CodeManager : MonoBehaviour
{
    public static CodeManager instance;
    public static string secretCode;

    [Header("Note UI")]
    public GameObject noteUI;
    public TextMeshProUGUI codeText;
    public float interactRange = 2f;
    public Transform player;

    bool playerNearNote = false;
    bool noteVisible = false;

    void Awake()
    {
        instance = this;
        // Generate random 4 digit code
        secretCode = Random.Range(1000, 9999).ToString();
        Debug.Log("Secret Code: " + secretCode);
    }

    void Start()
    {
        if (noteUI != null)
            noteUI.SetActive(false);
        else
            Debug.LogWarning("noteUI is not assigned!");

        if (codeText != null)
            codeText.text = "Code: " + secretCode;
        else
            Debug.LogWarning("codeText is not assigned!");
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned in CodeManager!");
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);
        playerNearNote = dist < interactRange;

        if (playerNearNote && Input.GetKeyDown(KeyCode.E))
        {
            noteVisible = !noteVisible;
            if (noteUI != null)
                noteUI.SetActive(noteVisible);
        }

        if (!playerNearNote && noteVisible)
        {
            noteVisible = false;
            if (noteUI != null)
                noteUI.SetActive(false);
        }
    }
}