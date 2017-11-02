using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour 
{
    public GameObject pauseMenu;
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject brightnessSlider;

    public bool gamePaused;

    void Start()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gamePaused = true;
        MainMenuManager.Instance.SetupBrightness(brightnessSlider);
        MainMenuManager.Instance.SetupBGM(bgmSlider);
        MainMenuManager.Instance.SetupSFX(sfxSlider);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gamePaused = false;
    }
}
