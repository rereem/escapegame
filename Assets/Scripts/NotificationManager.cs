using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public float displayTime = 3f;
    public float fadeSpeed = 2f;

    private Coroutine current;
    private RectTransform rt;
    private Vector2 originalPos;

    private void Awake()
    {
        instance = this;

        rt = notificationPanel.GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition; //  store correct position once

        notificationPanel.SetActive(false);
    }

    public static void Show(string message)
    {
        if (instance != null)
            instance.ShowNotification(message);
    }

    public void ShowNotification(string message)
    {
        if (current != null)
            StopCoroutine(current);

        //  reset position every time (prevents bug)
        rt.anchoredPosition = originalPos;

        current = StartCoroutine(DisplayNotification(message));
    }

    IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);

        Vector2 shownPos = originalPos;

        float padding = 20f;
        float panelWidth = rt.rect.width;

        Vector2 hiddenPos = shownPos + new Vector2(-(panelWidth + padding), 0f);

        rt.anchoredPosition = hiddenPos;

        float t = 0f;

        // 👉 Slide IN
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            rt.anchoredPosition = Vector2.Lerp(hiddenPos, shownPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        t = 0f;

        // 👉 Slide OUT
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            rt.anchoredPosition = Vector2.Lerp(shownPos, hiddenPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        notificationPanel.SetActive(false);
    }
}