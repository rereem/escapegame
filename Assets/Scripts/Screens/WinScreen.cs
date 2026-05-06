using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreen : MonoBehaviour
{
    public Image blackOverlay;
    public Image youWonImage;
    public float fadeSpeed = 0.5f;
    public AudioManager audioManager;
    public GameObject winScreenUI; // drag the Canvas WinScreen panel here
    public GameObject HealthBarUI; 


    public static WinScreen instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player")) return;

        if (!GameManager.instance.hasKey) return;

        ShowWinScreen();
    }

    public void ShowWinScreen()
    {
        winScreenUI.SetActive(true); // activate UI not the wall!
        HealthBarUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        if (audioManager != null)
            audioManager.StopMusic();

        blackOverlay.color = new Color(0, 0, 0, 1);
        youWonImage.color = new Color(1, 1, 1, 0);

        yield return new WaitForSecondsRealtime(1f);

        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.unscaledDeltaTime * fadeSpeed;
            blackOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            youWonImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}