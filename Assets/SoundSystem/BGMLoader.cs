using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMLoader : MonoBehaviour
{
	public AudioClipID bgm;

	void Start()
	{
		SoundManagerScript.Instance.PlayBGM(bgm);
	}
}
