using UnityEngine;

public class UIManager : MonoBehaviour
{
    //public PauseMenu pauseMenu;
    //public DeathMenu deathMenu;

    public GameObject deathUI; 
    public GameObject winUI; 
    public GameObject pauseUI;
    public GameObject inventory;

    public bool isPaused = false; // game is initially not paused
    // in main menu ui manager the isPasue should also be set again to false once <<start>> so no errors happens
    private void Start()
    {
        deathUI.SetActive(false);
        winUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    public void ActivateDeathUI()
    {
        deathUI.SetActive(true);
        inventory.SetActive(false);
        isPaused = !isPaused;

        Time.timeScale = 0;
    }

    public void ActivateWinUI()
    {
        winUI.SetActive(true);
        inventory.SetActive(false);
        isPaused = !isPaused;

        Time.timeScale = 0; 
    }

    public void ActivatePauseUI()
    {
        isPaused = !isPaused; 
        pauseUI.SetActive(isPaused); 

        Time.timeScale = isPaused ? 0 : 1; 
    }


}

