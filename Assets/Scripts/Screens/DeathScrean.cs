using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public Image youDiedImage;
    public float fadeSpeed = 1f;
    public AudioManager audioManager;
    public GameObject HealthBarUI;
    public GameObject buttons;
    public static DeathScreen instance;

    public void Restart()
    {
        GameEnvironment.Reset(); // ADD THIS — clears old checkpoint references
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("startmenu"); // your start menu scene name
    }
    private void Awake()
    {
        instance = this;
    }

    public void ShowDeathScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        if (HealthBarUI != null)
            HealthBarUI.SetActive(false);

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            youDiedImage.color = new Color(1, 1, 1, alpha);
            if (audioManager != null)
                audioManager.FadeMusicOut(1f - alpha);
            yield return null;
        }

        if (audioManager != null)
            audioManager.StopMusic();

        // AFTER fade finishes
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        buttons.SetActive(true);
    }
}