using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // player will pause = time will pause
    // resume       : time back and 
    // reload level
    // main menu


    public GameObject pauseMenu;

    public void PauseGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(true);
    }
    public void ResumePressed()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RetryPressed()
    {
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
