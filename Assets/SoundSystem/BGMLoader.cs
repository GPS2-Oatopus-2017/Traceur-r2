using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMLoader : MonoBehaviour
{
	void Start()
	{
		//SoundManagerScript.Instance.bgmAudioSource.Stop();
		//Main Menu
		if(SceneManager.GetActiveScene().name == "MainMenu")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU, true);
		}
		else
			SoundManagerScript.Instance.StopBGM(AudioClipID.BGM_MAIN_MENU);
		//Win Scene
		if(SceneManager.GetActiveScene().name == "WinScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_WIN, true);
		}
//		else
//			SoundManagerScript.Instance.StopBGM(AudioClipID.BGM_WIN);
		//Lose Scene
		if(SceneManager.GetActiveScene().name == "LoseScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_LOSE, true);
		}
//		else
//			SoundManagerScript.Instance.StopBGM(AudioClipID.BGM_LOSE);
	}
}
