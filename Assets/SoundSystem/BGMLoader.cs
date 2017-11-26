using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMLoader : MonoBehaviour
{
	void Start()
	{
		SoundManagerScript.Instance.bgmAudioSource.Stop();
		//Main Menu
		if(SceneManager.GetActiveScene().name == "MainMenu")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU, true);
		}
		//Win Scene
		else if(SceneManager.GetActiveScene().name == "WinScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_WIN, true);
		}
		//Lose Scene
		else if(SceneManager.GetActiveScene().name == "LoseScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_LOSE, true);
		}
	}
}
