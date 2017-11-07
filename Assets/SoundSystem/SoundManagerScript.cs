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
	BGM_TUTO = 4,

	// UI
	SFX_UI_BUTTON = 5,
	SFX_TYPING = 6,

	//SFXs
	SFX_RUNNING = 7,
	SFX_DRONE_HOVER = 8,
	SFX_SR_OPENDOOR = 9,
	SFX_SR_CLOSEDOOR = 10
}

//Audioclip Type
public enum AudioClipPriority
{
	FEEDBACK = 0,
	IMMERSION = 1,
	ENTERTAINMENT = 2,

	TOTAL
}

[System.Serializable]
public class AudioClipInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioClip;
	public AudioClipPriority priority;
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

	[Header("Priority Table")]
	public float[] volumeMultiplier = new float[(int)AudioClipPriority.TOTAL];

	[Header("Clips")]
	public List<AudioClipInfo> audioClipInfoList = new List<AudioClipInfo>();

	[Header("Sources")]
	public AudioSource bgmAudioSource;
	public AudioClipID bgmAudioClipID;
//	public AudioClipPriority bgmPriority;
	public AudioSource sfxAudioSource2DOneshot;
	public AudioClipID sfxAudioClipID2DOneShot;
	public List<AudioSource> sfxAudioSourceList2DLoop = new List<AudioSource>();
	public List<AudioClipID> sfxAudioClipID2DLoop = new List<AudioClipID>();
//	public List<AudioClipPriority> sfxPriority2D = new List<AudioClipPriority>();
	public List<AudioSource> sfxAudioSourceList3D = new List<AudioSource>();
	public List<AudioClipID> sfxAudioClipID3D = new List<AudioClipID>();
//	public List<AudioClipPriority> sfxPriority3D = new List<AudioClipPriority>();

	// Preload before any Start() rins in other scripts
	void Awake () 
	{
		//Singleton Setup
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);

		DontDestroyOnLoad(gameObject);

//		AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource>();
//
//		for(int i = 0; i < audioSourceList.Length; i++)
//		{
//			if(audioSourceList[i].gameObject.tag == "SoundManager")
//			{
//				continue;
//			}
//
//			audioSourceList[i].spatialBlend = 1.0f; // For 3D Sounds
//			sfxAudioSourceList3D.Add(audioSourceList[i]);
//		}
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

	float FindAudioClipVolumeMultipliers(AudioClipID audioClipID)
	{
		for(int i=0; i<audioClipInfoList.Count; i++)
		{
			if(audioClipInfoList[i].audioClipID == audioClipID)
			{
				return volumeMultiplier[(int)audioClipInfoList[i].priority];
			}
		}

		Debug.LogError("Cannot Find Audio Clip : " + audioClipID);

		return 1.0f;
	}

	AudioSource FindAudioSource3D(GameObject go)
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
		bgmAudioClipID = audioClipID;
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
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null)
		{
			source = go.AddComponent<AudioSource>();
			source.loop = true;
			source.spatialBlend = 1.0f; //For 3D sounds
			sfxAudioSourceList3D.Add(source);
			sfxAudioClipID3D.Add(audioClipID);
		}

		source.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
		source.PlayOneShot(clipToPlay, source.volume);
	}

	public void PlayLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null)
		{
			source = go.AddComponent<AudioSource>();
			source.loop = true;
			source.spatialBlend = 1.0f; //For 3D sounds
			sfxAudioSourceList3D.Add(source);
			sfxAudioClipID3D.Add(audioClipID);
		}

		if(source.clip != clipToPlay)
		{
			source.clip = clipToPlay;
		}

		if(source.isPlaying)
		{
			return;
		}

		source.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
		source.Play();
	}

	public void PauseLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null) return;

		if(source.clip == clipToPause)
		{
			source.Pause();
		}
	}	

	public void StopLoopingSFX(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null) return;

		if(source.clip == clipToStop)
		{
			source.Stop();
		}
	}

	public void ChangePitchLoopingSFX(AudioClipID audioClipID, float value, GameObject go)
	{
		AudioClip clipToChange = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null) return;

		if(source.clip == clipToChange)
		{
			source.pitch = value;
		}
	}

	//! SOUND EFFECTS (SFX) 2D
	public void PlaySFX2D(AudioClipID audioClipID)
	{
		sfxAudioClipID2DOneShot = audioClipID;
		sfxAudioSource2DOneshot.PlayOneShot(FindAudioClip(audioClipID), sfxVolume * FindAudioClipVolumeMultipliers(audioClipID));
	}

	public void PlayLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2DLoop.Count; i++)
		{
			if(sfxAudioSourceList2DLoop[i].clip == clipToPlay)
			{
				if(sfxAudioSourceList2DLoop[i].isPlaying)
				{
					return;
				}

				sfxAudioSourceList2DLoop[i].volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
				sfxAudioSourceList2DLoop[i].Play();
				return;
			}
		}

		AudioSource newInstance = gameObject.AddComponent<AudioSource>();
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
		newInstance.loop = true;
		newInstance.spatialBlend = 0.0f; //For 2D sounds
		newInstance.Play();
		sfxAudioSourceList2DLoop.Add(newInstance);
		sfxAudioClipID2DLoop.Add(audioClipID);
	}

	public void PauseLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2DLoop.Count; i++)
		{
			if(sfxAudioSourceList2DLoop[i].clip == clipToPause)
			{
				sfxAudioSourceList2DLoop[i].Pause();
				return;
			}
		}
	}	

	public void StopLoopingSFX2D(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2DLoop.Count; i++)
		{
			if(sfxAudioSourceList2DLoop[i].clip == clipToStop)
			{
				sfxAudioSourceList2DLoop[i].Stop();
				return;
			}
		}
	}

	public void ChangePitchLoopingSFX2D(AudioClipID audioClipID, float value)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList2DLoop.Count; i++)
		{
			if(sfxAudioSourceList2DLoop[i].clip == clipToStop)
			{
				sfxAudioSourceList2DLoop[i].pitch = value;
				return;
			}
		}
	}

	public void SetBGMVolume(float value)
	{
		bgmVolume = value;
		UpdateBGMVolume();
	}

	public void SetSFXVolume(float value)
	{
		sfxVolume = value;
		UpdateSFXVolume();
	}

	public void UpdateBGMVolume()
	{
		bgmAudioSource.volume = bgmVolume * FindAudioClipVolumeMultipliers(bgmAudioClipID);
	}

	public void UpdateSFXVolume()
	{
		sfxAudioSource2DOneshot.volume = sfxVolume * FindAudioClipVolumeMultipliers(sfxAudioClipID2DOneShot);
		for(int i = 0; i < sfxAudioSourceList2DLoop.Count; i++)
		{
			sfxAudioSourceList2DLoop[i].volume = sfxVolume * FindAudioClipVolumeMultipliers(sfxAudioClipID2DLoop[i]);
		}
		for(int i = 0; i < sfxAudioSourceList3D.Count; i++)
		{
			sfxAudioSourceList3D[i].volume = sfxVolume * FindAudioClipVolumeMultipliers(sfxAudioClipID3D[i]);
		}
	}
}