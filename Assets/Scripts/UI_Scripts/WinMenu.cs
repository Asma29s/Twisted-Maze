using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public GameObject winMenu;

    public void PlayerWon()
    {
        winMenu.SetActive(true);
        // add sound
        Time.timeScale = 0f; // freeze time
    }
    public void RetryPressed()
    {
        winMenu.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
