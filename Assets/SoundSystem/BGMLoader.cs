using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMLoader : MonoBehaviour
{
	private static bool alreadySetup = false;

	void Awake()
	{
		if(!alreadySetup)
		{
			SceneManager.activeSceneChanged += CheckBGM; // subscribe
			alreadySetup = true;
		}
	}

	void CheckBGM(Scene previousScene, Scene newScene)
	{
		SoundManagerScript.Instance.bgmAudioSource.Stop();
		//Main Menu
		if(newScene.name == "MainMenu")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU, true);
		}
		//Win Scene
		if(newScene.name == "WinScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_WIN, true);
		}
		//Lose Scene
		if(newScene.name == "LoseScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_LOSE, true);
		}
		//Finish Credit
		if(newScene.name == "EndCreditScene")
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_FINISH_CREDIT, true);
		}
	}

//	void OnDestroy()
//	{
//		SceneManager.activeSceneChanged -= CheckBGM; // unsubscribe
//	}
}
