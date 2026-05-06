using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    public Image youDiedImage;
    public float fadeSpeed = 1f;
    public AudioManager audioManager; // ✅ drag your Audio GameObject here

    public static DeathScreen instance;

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
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            youDiedImage.color = new Color(1, 1, 1, alpha); // image fades in
            audioManager.FadeMusicOut(1f - alpha); // ✅ music fades out
            yield return null;
        }
        audioManager.StopMusic(); // ✅ fully stop at the end
    }
}