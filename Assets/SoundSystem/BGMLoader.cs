using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMLoader : MonoBehaviour
{
	void Start()
	{
		if(SceneManager.GetActiveScene().name == "MainMenu")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
		}

		if(SceneManager.GetActiveScene().name == "Tutorial")
		{
			SoundManagerScript.Instance.StopBGM(AudioClipID.BGM_MAIN_MENU);
		}

		if(SceneManager.GetActiveScene().name == "WinScene")
		{
			SoundManagerScript.Instance.PlayOneShotSFX2D(AudioClipID.BGM_WIN);
		}

		if(SceneManager.GetActiveScene().name == "LoseScene")
		{
			SoundManagerScript.Instance.PlayOneShotSFX2D(AudioClipID.BGM_LOSE);
		}
	}
}
