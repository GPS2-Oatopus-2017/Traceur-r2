using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour 
{
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public float brightness = 0.75f;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public Image brightnessMask;

    private static MenuSettings mInstance = null;
    
    public static MenuSettings Instance
    {
        get
        {
            if(mInstance == null)
            {
                GameObject tempObject = GameObject.FindWithTag("SettingManager");
    
                if(tempObject == null)
                {
                    Debug.LogError("ManagerController is missing, the game cannot continue to work.");
                    Debug.Break();
                }
                else
                {
                  mInstance = tempObject.GetComponent<MenuSettings>();
                }
            }
            return mInstance;
        }
    }
    public static bool CheckInstanceExist()
    {
        return mInstance;
    }

    void Awake()
    {
        if(MenuSettings.CheckInstanceExist())
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        bgmAudioSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxAudioSource.volume = sfxVolume;
    }

    public void SetBrightness(float value)
    {
        brightness = value;
        brightnessMask.color = new Color(0, 0, 0, 1 - brightness);
    }
}
