using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Button_Start()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Button_Quit()
    {
        Application.Quit();
    }
}