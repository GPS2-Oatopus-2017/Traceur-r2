using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audioSourceTracker : MonoBehaviour {

	public AudioSource[] audioSources;
	public AudioClipID[] audioClipIDList;

	// Use this for initialization
	void Awake ()
	{
		for(int i = 0; i < audioSources.Length; i++)
		{
			SoundManagerScript.Instance.sfxAudioSourceList3D.Add(audioSources[i]);
			SoundManagerScript.Instance.sfxAudioClipID3D.Add(audioClipIDList[i]);
		}
		SoundManagerScript.Instance.UpdateBGMVolume();
		SoundManagerScript.Instance.UpdateSFXVolume();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnDestroy()
//	{
//		SoundManagerScript.Instance.sfxAudioSourceList3D.Clear();
//		SoundManagerScript.Instance.sfxAudioSourceList3D.Capacity = 0;
//	}
}
