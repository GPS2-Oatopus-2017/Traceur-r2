using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Iinteractable 
{
	void Interacted();
}

public class SwitchScript : MonoBehaviour, Iinteractable
{
	public void Interacted()
	{
		TurnLever ();
	}
		
	public GameObject otherSwitch;
	public GameObject fence;
	GameObject lever;
	GameObject otherLever;
	GameObject fenceBar;
	ElectricWallScript wall;
	SwitchScript other;
	public bool isOn;


	void Start ()
	{
		isOn = true;
		fence.SetActive (true);
		lever = this.transform.GetChild(1).gameObject;
		otherLever = otherSwitch.transform.GetChild(1).gameObject;
		other = otherSwitch.GetComponent<SwitchScript>();
		fenceBar = fence.transform.GetChild(1).gameObject;
		wall = fence.GetComponent<ElectricWallScript>();
		SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_FENCE_IDLE, fenceBar, true);
	}
	

	public void TurnLever()
	{
		if(isOn == false) 
		{
			lever.transform.localEulerAngles = new Vector3(-90,0,0);
			otherLever.transform.localEulerAngles = new Vector3(-90,0,0);
			isOn = true;
			other.isOn = true;
			fenceBar.SetActive (true);
			SoundManagerScript.Instance.PlaySFX3D(AudioClipID.SFX_FENCE_IDLE, fenceBar, true);
			wall.isActived = true;
		} 
		else if(isOn == true) 
		{
			lever.transform.localEulerAngles = new Vector3(0,0,0);
			otherLever.transform.localEulerAngles = new Vector3(0,0,0);
			isOn = false;
			other.isOn = false;
			SoundManagerScript.Instance.StopSFX3D(AudioClipID.SFX_FENCE_IDLE, fenceBar);
			fenceBar.SetActive (false);
			wall.isActived = false;
		}

		SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_SWITCH, gameObject);
		SoundManagerScript.Instance.PlayOneShotSFX3D(AudioClipID.SFX_SWITCH, otherSwitch);
	}
}
