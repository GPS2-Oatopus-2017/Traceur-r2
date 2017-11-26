﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audioSourceTracker : MonoBehaviour {

	public AudioSource[] audioSources;

	// Use this for initialization
	void Awake ()
	{
		for(int i = 0; i < audioSources.Length; i++)
		{
			SoundManagerScript.Instance.sfxAudioSourceList3D.Add(audioSources[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
