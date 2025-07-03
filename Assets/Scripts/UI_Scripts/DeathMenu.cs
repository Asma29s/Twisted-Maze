using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // when player collides with the enemy:
    // 1) Death menu will start, time will stop
    // if pressed replay: reload scene and time is back 
    // if main menu pressed: load main menu scene

    public GameObject deathMenu;

    public void PlayerDied()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f; // freeze time
    }
    public void RetryPressed()
    {
        deathMenu.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}


