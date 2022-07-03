using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("CardTable");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
