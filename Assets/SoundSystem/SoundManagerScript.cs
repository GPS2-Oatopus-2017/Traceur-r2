using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AudioClipID
{
	// Main Menu
	BGM_MAIN_MENU = 0,
	SFX_MAIN_MENU,

	// Lose & Win
	BGM_LOSE,
	BGM_WIN,

	// Levels
	BGM_TUTO,
	BGM_LVL_ONE,

	BGM_FINISH_CREDIT,

	// UI
	SFX_UI_BUTTON,

	//SFXs
	SFX_RUNNING,
	SFX_DRONE_HOVER,
	SFX_DOOR,
    SFX_COUNTDOWN,
	SFX_FENCE,
	SFX_SWITCH,
	SFX_JUMP,
	SFX_LAND,
	SFX_ROLL,
	SFX_KNOCK_ONE,
	SFX_KNOCK_TWO,
	SFX_HIT,
	SFX_DRONE_ALERT,
	SFX_CHARGE,
	SFX_LASER,
	SFX_FENCE_ACTIVE,
	SFX_FENCE_IDLE,
	SFX_MD_BEEP,
	SFX_MD_ACIVATED,
	SFX_MD_DEACTIVATED,

    //Amanda's
    A_BS_ONE,
    A_BS_TWO,
    A_BS_THREE,

    A_IL_ONE,
    A_IL_TWO,
    A_IL_THREE,
    A_IL_FOUR,
    A_IL_FIVE,
    A_IL_SIX,
    A_IL_SEVEN,
    A_IL_EIGHT,

    A_WS_ONE,
    A_WS_TWO,
    A_WS_THREE,
    A_WS_FOUR,

    A_LS_ONE,
    A_LS_TWO,
    A_LS_THREE,
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
	public void PlayOneShotSFX3D(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null)
		{
			source = go.AddComponent<AudioSource>();
			source.spatialBlend = 1.0f; //For 3D sounds
			sfxAudioSourceList3D.Add(source);
			sfxAudioClipID3D.Add(audioClipID);
		}

		source.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
		source.PlayOneShot(clipToPlay, source.volume);
    }

    public void PlaySFX3D(AudioClipID audioClipID, GameObject go, bool loop = true)
    {
        AudioClip clipToPlay = FindAudioClip(audioClipID);

        AudioSource source = FindAudioSource3D(go);

        if(source == null)
        {
            source = go.AddComponent<AudioSource>();
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

        source.loop = loop;
        source.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
        source.Play();
    }

	public void PauseSFX3D(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null) return;

		if(source.clip == clipToPause)
		{
			source.Pause();
		}
	}	

	public void StopSFX3D(AudioClipID audioClipID, GameObject go)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		AudioSource source = FindAudioSource3D(go);

		if(source == null) return;

		if(source.clip == clipToStop)
		{
			source.Stop();
		}
    }

    public void ChangeLoopSFX3D(AudioClipID audioClipID, bool value, GameObject go)
    {
        AudioClip clipToChange = FindAudioClip(audioClipID);

        AudioSource source = FindAudioSource3D(go);

        if(source == null) return;

        if(source.clip == clipToChange)
        {
            source.loop = value;
        }
    }

    public void ChangePitchSFX3D(AudioClipID audioClipID, float value, GameObject go)
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
	public void PlayOneShotSFX2D(AudioClipID audioClipID)
	{
		sfxAudioClipID2DOneShot = audioClipID;
		sfxAudioSource2DOneshot.PlayOneShot(FindAudioClip(audioClipID), sfxVolume * FindAudioClipVolumeMultipliers(audioClipID));
	}

    public void PlaySFX2D(AudioClipID audioClipID, bool loop = true)
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
                sfxAudioSourceList2DLoop[i].loop = loop;
				sfxAudioSourceList2DLoop[i].volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
				sfxAudioSourceList2DLoop[i].Play();
				return;
			}
		}

		AudioSource newInstance = gameObject.AddComponent<AudioSource>();
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume * FindAudioClipVolumeMultipliers(audioClipID);
		newInstance.loop = loop;
		newInstance.spatialBlend = 0.0f; //For 2D sounds
		newInstance.Play();
		sfxAudioSourceList2DLoop.Add(newInstance);
		sfxAudioClipID2DLoop.Add(audioClipID);
	}

	public void PauseSFX2D(AudioClipID audioClipID)
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

	public void StopSFX2D(AudioClipID audioClipID)
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

    public void ChangeLoopSFX2D(AudioClipID audioClipID, bool value)
    {
        AudioClip clipToStop = FindAudioClip(audioClipID);

        for(int i=0; i<sfxAudioSourceList2DLoop.Count; i++)
        {
            if(sfxAudioSourceList2DLoop[i].clip == clipToStop)
            {
                sfxAudioSourceList2DLoop[i].loop = value;
                return;
            }
        }
    }

    public void ChangePitchSFX2D(AudioClipID audioClipID, float value)
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
		bgmAudioSource.volume = bgmVolume;// * FindAudioClipVolumeMultipliers(bgmAudioClipID);
	}

	public void UpdateSFXVolume()
	{
		sfxAudioSource2DOneshot.volume = sfxVolume;// * FindAudioClipVolumeMultipliers(sfxAudioClipID2DOneShot);
		for(int i = 0; i < sfxAudioSourceList2DLoop.Count; i++)
		{
			sfxAudioSourceList2DLoop[i].volume = sfxVolume;// * FindAudioClipVolumeMultipliers(sfxAudioClipID2DLoop[i]);
		}
		for(int i = 0; i < sfxAudioSourceList3D.Count; i++)
		{
			sfxAudioSourceList3D[i].volume = sfxVolume;// * FindAudioClipVolumeMultipliers(sfxAudioClipID3D[i]);
		}
	}
}