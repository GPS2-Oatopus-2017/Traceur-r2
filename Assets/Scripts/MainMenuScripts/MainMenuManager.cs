using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Settings")]
    public GameObject[] menuWindows;
    public string startingLevel;
    public GameObject[] sliders;

    public static MainMenuManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetupBrightness(sliders[0]);
        SetupBGM(sliders[1]);
        SetupSFX(sliders[2]);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startingLevel); //Start Game
    }

    public void OpenMenu(int menu)
    {
        menuWindows[menu].SetActive(true); //Activate Menu
    }

    public void CloseMenu(int menu)
    {
        menuWindows[menu].SetActive(false); //Deactivate Menu
    }

    public void SetupBGM(GameObject slider)  // Will work on this with Syabil soon
	{
		slider.GetComponent<Slider>().value = SoundManagerScript.Instance.bgmVolume;
        ChangeBGM(slider);
    }

    public void SetupSFX(GameObject slider)
    {
		slider.GetComponent<Slider>().value = SoundManagerScript.Instance.sfxVolume;
        ChangeSFX(slider);
    }

    public void ChangeBGM(GameObject slider)
    {
		SoundManagerScript.Instance.SetBGMVolume(slider.GetComponent<Slider>().value);
        Debug.Log("Current BGM value is : " + slider.GetComponent<Slider>().value);
    }

    public void ChangeSFX(GameObject slider)
    {
		SoundManagerScript.Instance.SetSFXVolume(slider.GetComponent<Slider>().value);
        Debug.Log("Current SFX value is : " + slider.GetComponent<Slider>().value);
    }

    public void SetupBrightness(GameObject slider) //Set initial screen brightness
    {
		slider.GetComponent<Slider>().value = MenuSettings.Instance.brightness;
        ChangeBrightness(slider);
    }

    public void ChangeBrightness(GameObject slider) //Alter screen brightness value
    {
        MenuSettings.Instance.SetBrightness(slider.GetComponent<Slider>().value);
        Debug.Log("Current Brightness value is : " + slider.GetComponent<Slider>().value);
    }

	public void onClick()
	{
		SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_UI_BUTTON);
	}

	public void sliderchangeSFX()
	{
		SoundManagerScript.Instance.PlaySFX2D(AudioClipID.SFX_MAIN_MENU);
	}
}
