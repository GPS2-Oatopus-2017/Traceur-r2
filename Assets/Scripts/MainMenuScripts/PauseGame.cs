using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour 
{
    public GameObject pauseMenu;
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject brightnessSlider;

    public List<AudioSource> sourcesToBePaused;

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

        sourcesToBePaused.Clear();

        if(SoundManagerScript.Instance.bgmAudioSource.isPlaying)
        {
            sourcesToBePaused.Add(SoundManagerScript.Instance.bgmAudioSource);
        }

        for(int i = 0; i < SoundManagerScript.Instance.sfxAudioSourceList2DLoop.Count; i++)
        {
            if(SoundManagerScript.Instance.sfxAudioSourceList2DLoop[i].isPlaying)
            {
                sourcesToBePaused.Add(SoundManagerScript.Instance.sfxAudioSourceList2DLoop[i]);
            }
        }

        for(int i = 0; i < sourcesToBePaused.Count; i++)
        {
            sourcesToBePaused[i].Pause();
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gamePaused = false;

        for(int i = 0; i < sourcesToBePaused.Count; i++)
        {
            sourcesToBePaused[i].Play();
        }

        sourcesToBePaused.Clear();
    }
}
