using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AudioClipID
{
	// Main Menu
	BGM_MAIN_MENU = 0,
	SFX_MAIN_MENU = 1,

	// Lose & Win
	BGM_LOSE = 2,
	BGM_WIN = 3,

	// Levels
	BGM_LEVEL1 = 4,

	// UI
	SFX_UI_BUTTON = 5,

	//SFXs
	SFX_RUNNING = 6,
	SFX_DRONE_HOVER = 7,
	SFX_SR_OPENDOOR = 8,
	SFX_SR_CLOSEDOOR = 9
}

[System.Serializable]
public class AudioClipInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioClip;
}

public class SoundManagerScript : MonoBehaviour 
{
	//Singleton Setup
	private static SoundManagerScript mInstance;
	public static SoundManagerScript Instance
	{
		get { return mInstance; }
	}

	[Range(0.0f, 1.0f)]
	public float bgmVolume = 1.0f;
	[Range(0.0f, 1.0f)]
	public float sfxVolume = 1.0f;


	public List<AudioClipInfo> audioClipInfoList = new List<AudioClipInfo>();

	public AudioSource bgmAudioSource;
	public AudioSource sfxAudioSource2D;
	public List<AudioSource> sfxAudioSourceList2D = new List<AudioSource>();
	public List<AudioSource> sfxAudioSourceList3D = new List<AudioSource>();

	// Preload before any Start() rins in other scripts
	void Awake () 
	{
		//Singleton Setup
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);

		DontDestroyOnLoad(gameObject);

		AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource>();

		for(int i = 0; i < audioSourceList.Length; i++)
		{
			if(audioSourceList[i].gameObject.tag == "SoundManager")
			{
				continue;
			}

			audioSourceList[i].spatialBlend = 1.0f; // For 3D Sounds
			sfxAudioSourceList3D.Add(audioSourceList[i]);
		}
	}

	AudioClip FindAudioClip(AudioClipID audioClipID)
	{
		for(int i=0; i<audioClipInfoList.Count; i++)
		{
			if(audioClipInfoList[i].audioClipID == audioClipID)
			{
				return audioClipInfoList[i].audioClip;
			}
		}

		Debug.LogError("Cannot Find Audio Clip : " + audioClipID);

		return null;
	}

	AudioSource FindAudioSource(GameObject go)
	{
		for(int i=0; i<sfxAudioSourceList3D.Count; i++)
		{
			if(sfxAudioSourceList3D[i].gameObject == go)
			{
				return sfxAudioSourceList3D[i];
			}
		}

		Debug.LogError("Cannot Find Audio Source in : " + go.name);

		return null;
	}

	//! BACKGROUND MUSIC (BGM)
	public void PlayBGM(AudioClipID audioClipID)
	{
		bgmAudioSource.clip = FindAudioClip(audioClipID);
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	public void PauseBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Pause();
		}
	}

	public void StopBGM(AudioClipID audioClipID)
	{
		bgmAudioSource.clip = FindAudioClip(audioClipID);
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Stop();
	}


	//! SOUND EFFECTS (SFX)
	public void PlaySFX(AudioClipID audioClipID, GameObject go)
	{
		AudioSource source = FindAudioSource(go);
		if(source != null)
		{
			source.PlayOneShot(FindAudioClip(audioClipID), sfxVolume);
		}
	}

	public void PlayLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource(go);

		if(source == null)
		{
			AudioSource newInstance = go.AddComponent<AudioSource>();
			newInstance.clip = clipToPlay;
			newInstance.volume = sfxVolume;
			newInstance.loop = true;
			newInstance.spatialBlend = 1.0f; //For 3D sounds
			newInstance.Play();
			sfxAudioSourceList3D.Add(newInstance);
		}
		else
		{
			if(source.clip != clipToPlay)
			{
				source.clip = clipToPlay;
			}

			if(source.isPlaying)
			{
				return;
			}

			source.volume = sfxVolume;
			source.Play();
			return;
		}
	}

	public void PauseLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource(go);

		if(source == null) return;

		if(source.clip == clipToPause)
		{
			source.Pause();
		}
	}	

	public void StopLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource(go);

		if(source == null) return;

		if(source.clip == clipToStop)
		{
			source.Stop();
		}
	}

	public void ChangePitchLoopingSFX(AudioClipID audioClipID, float value, GameObject go)
	{
		AudioClip clipToChange = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource(go);

		if(source == null) return;

		if(source.clip == clipToChange)
		{
			source.pitch = value;
		}
	}

	//! SOUND EFFECTS (SFX) 2D
	public void PlaySFX2D(AudioClipID audioClipID)
	{
		sfxAudioSource2D.PlayOneShot(FindAudioClip(audioClipID), sfxVolume);
	}

	public void PlayLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2D.Count; i++)
		{
			if(sfxAudioSourceList2D[i].clip == clipToPlay)
			{
				if(sfxAudioSourceList2D[i].isPlaying)
				{
					return;
				}

				sfxAudioSourceList2D[i].volume = sfxVolume;
				sfxAudioSourceList2D[i].Play();
				return;
			}
		}

		AudioSource newInstance = gameObject.AddComponent<AudioSource>();
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume;
		newInstance.loop = true;
		newInstance.spatialBlend = 0.0f; //For 2D sounds
		newInstance.Play();
		sfxAudioSourceList2D.Add(newInstance);
	}

	public void PauseLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2D.Count; i++)
		{
			if(sfxAudioSourceList2D[i].clip == clipToPause)
			{
				sfxAudioSourceList2D[i].Pause();
				return;
			}
		}
	}	

	public void StopLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2D.Count; i++)
		{
			if(sfxAudioSourceList2D[i].clip == clipToStop)
			{
				sfxAudioSourceList2D[i].Stop();
				return;
			}
		}
	}

	public void ChangePitchLoopingSFX2D(AudioClipID audioClipID, float value)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2D.Count; i++)
		{
			if(sfxAudioSourceList2D[i].clip == clipToStop)
			{
				sfxAudioSourceList2D[i].pitch = value;
				return;
			}
		}
	}

	public void SetBGMVolume(float value)
	{
		bgmVolume = value;
		bgmAudioSource.volume = bgmVolume;
	}

	public void SetSFXVolume(float value)
	{
		sfxVolume = value;
		sfxAudioSource2D.volume = sfxVolume;
		for(int i = 0; i < sfxAudioSourceList2D.Count; i++)
		{
			sfxAudioSourceList2D[i].volume = sfxVolume;
		}
		for(int i = 0; i < sfxAudioSourceList3D.Count; i++)
		{
			sfxAudioSourceList3D[i].volume = sfxVolume;
		}
	}
}