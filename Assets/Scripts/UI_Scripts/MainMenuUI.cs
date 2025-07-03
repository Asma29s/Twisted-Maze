using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartPressed()
    {
        SceneManager.LoadScene("SamScene2");
    }
    public void QuitPressed()
    {
        Application.Quit();
    }
}
